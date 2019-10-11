using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour , IOnEventCallback
{
    static Manager _instance;
    public Test stateHolder { get; private set; }
    List<Player> stillNotPlayed = new List<Player>();
    GameObject player;
    public bool IsDeciding { get; set; }

    [SerializeField]
    UIAnimTest anim;
    public UIAnimTest Anim => anim;

    [SerializeField]
    AbstractRoomState announce;

    [SerializeField]
    AbstractRoomState result;

    [SerializeField]
    AbstractRoomState start;

    [SerializeField]
    Text txtScore;

    [HideInInspector]
    public bool wasRight = false;
    public Dictionary<string, string> choices = new Dictionary<string, string>();
    public bool disableEvents = false;
    [HideInInspector]
    public string chosenStr;
    bool isFirst = true;

    public int roundLeft = 1;

    int point = 0;
    public int Point
    {
        get
        {
            return point;
        }
        set
        {
            point = value;
            txtScore.text = "Score : " + point;
            ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
            hash["Score"] = point;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void Init()
    {
        List<Player> players = new List<Player>();
        foreach(Player plyr in PhotonNetwork.CurrentRoom.Players.Values)
        {
            players.Add(plyr);
        }
        Player chosen = players[Random.Range(0, players.Count)];
        byte evCode = ConstEvents.STARTROUND;
        object content = new object[] { PhotonNetwork.LocalPlayer.UserId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Start()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;

        Point = 0;
        stateHolder = GameObject.FindObjectOfType<Test>();
        stateHolder.SwitchState(start);
    }

    public static Manager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject().AddComponent<Manager>();
        }

        return _instance;
    }

    public void Send(InputField iF)
    {
        byte evCode = ConstEvents.SENT;
        object content = new object[] { " - " + iF.text , PhotonNetwork.LocalPlayer.UserId};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
        iF.text = "";
    }

    public void Chose(string text)
    {
        chosenStr = text;
        byte evCode = ConstEvents.CHOSEN;
        object content = new object[] { text , choices[text]};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (disableEvents)
            return;

        byte eventCode = photonEvent.Code;

        if (eventCode == ConstEvents.STARTROUND)
        {
            txtScore.gameObject.SetActive(true);
            object[] data = (object[])photonEvent.CustomData;
            string str = (string)data[0];
            stateHolder.AddString(str);
            if (isFirst)
            {
                isFirst = false;
                IsDeciding = PhotonNetwork.LocalPlayer.UserId == str;
            }

            //Chose a random image
            if (IsDeciding)
            {
                ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
                hash.Add("imageId", Random.Range(0, 100));
                PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
                stateHolder.AddString("chosen");
            }

            stateHolder.SwitchState(announce);
        }

        if (eventCode == ConstEvents.SENT)
        {
            object[] data = (object[])photonEvent.CustomData;
            string str = (string)data[0];
            string id = (string)data[1];
            choices[str] = id;

            //Everyone has chosen
            if (choices.Count == PhotonNetwork.CurrentRoom.Players.Count - 1 && IsDeciding)
            {
                byte evCode = ConstEvents.TIMEUP;
                object content = new object[] {};
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                SendOptions sendOptions = new SendOptions { Reliability = true };
                PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
            }
        }

        if (eventCode == ConstEvents.TIMEUP)
        {
            stateHolder.NextState();
        }

        //Verdict
        if (eventCode == ConstEvents.CHOSEN)
        {
            object[] data = (object[])photonEvent.CustomData;
            string str = (string)data[0];
            if (!IsDeciding)
            {
                if (choices[str] == PhotonNetwork.LocalPlayer.UserId)
                {
                    wasRight = true;
                }
            }       
            StartCoroutine(DelayedSwitch());
            stateHolder.SwitchState(result);
            ((Result)result).SetChosenPhrases(str);
        }
    }

    public IEnumerator DelayedSwitch()
    {
        yield return new WaitForSeconds(10);
        stateHolder.NextState();
        IsDeciding = wasRight;
    }

    public void StartAgain()
    {
        stateHolder.SwitchState(announce);
    }
}
