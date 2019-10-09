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
    AbstractRoomState announce;

    [SerializeField]
    AbstractRoomState result;

    public bool wasRight = false;
    public Dictionary<string, string> choices = new Dictionary<string, string>();
    public bool disableEvents = false;
    [HideInInspector]
    public string chosenStr;

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

        stateHolder = GameObject.FindObjectOfType<Test>();
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
        object content = new object[] { iF.text , PhotonNetwork.LocalPlayer.UserId};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
        iF.text = "";
    }

    public void Chose(string text)
    {
        chosenStr = text;
        string id = choices[text];
        byte evCode = ConstEvents.CHOSEN;
        object content = new object[] { id };
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
            object[] data = (object[])photonEvent.CustomData;
            string str = (string)data[0];
            stateHolder.AddString(str);
            IsDeciding = PhotonNetwork.LocalPlayer.UserId == str;
            stateHolder.SwitchState(announce);
        }

        if (eventCode == ConstEvents.SENT)
        {
            object[] data = (object[])photonEvent.CustomData;
            string str = (string)data[0];
            string id = (string)data[1];
            stateHolder.AddString(str);
            if (IsDeciding)
                choices[str] = id;

            stateHolder.AddString(choices.Count == PhotonNetwork.CurrentRoom.Players.Count - 1 ? "True" : "False");

            //Everyone has chosen
            if (choices.Count == PhotonNetwork.CurrentRoom.Players.Count - 1)
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
            choices.Clear();
            stateHolder.AddString("cleared"); 
            object[] data = (object[])photonEvent.CustomData;
            string str = (string)data[0];
            if (!IsDeciding)
            {
                stateHolder.SwitchState(result);
                ((Result)result).SetRightness(str == PhotonNetwork.LocalPlayer.UserId);
            }       
            else
            {
                stateHolder.SwitchState(null);
                StartCoroutine(DelayedSwitch());
            }                
        }
    }

    public IEnumerator DelayedSwitch()
    {
        yield return new WaitForSeconds(3);
        Init();
    }
}
