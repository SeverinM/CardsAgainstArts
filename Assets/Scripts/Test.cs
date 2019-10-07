using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Test : MonoBehaviourPunCallbacks
{
    List<string> allLogs = new List<string>();
    Rect windowRect = new Rect(20, 20, 300, 800);

    [SerializeField]
    RoomDisplay rd;

    void AddString(string str)
    {
        allLogs.Add(str);
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.NickName = "Bata";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        AddString("Connected to server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        AddString("Joined lobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        AddString("Joined failed");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        AddString("Joined");
        rd.ComputeDisplay();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        rd.ComputeDisplay();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        rd.ComputeDisplay();
    }

    public override void OnLeftRoom()
    {
        rd.Clean();
    }

    void OnGUI()
    {
        windowRect = GUI.Window(0, windowRect, WindowFun, "Log");
    }

    void WindowFun(int windowId)
    {
        int count = 0;
        foreach(string str in allLogs)
        {
            GUI.Label(new Rect(20, (20 * count) + 20, 120, 20), str);
            count++;
        }
    }
}
