using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Manager : MonoBehaviour , IOnEventCallback
{
    static Manager _instance;
    public Test stateHolder { get; private set; }
    List<Player> stillNotPlayed = new List<Player>();
    GameObject player;
    public bool IsDeciding { get; set; }

    [SerializeField]
    AbstractRoomState announce;

    public void Init()
    {
        List<Player> players = new List<Player>();
        foreach(Player plyr in PhotonNetwork.CurrentRoom.Players.Values)
        {
            players.Add(plyr);
        }
        Player chosen = players[Random.Range(0, players.Count)];

        byte evCode = ConstEvents.STARTROUND;
        object content = chosen.UserId;
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

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == 1)
        {
            string data = (string)photonEvent.CustomData;
            IsDeciding = PhotonNetwork.LocalPlayer.UserId == data;
            stateHolder.SwitchState(announce);
        }
    }
}
