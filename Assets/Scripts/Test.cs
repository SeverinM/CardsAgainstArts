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
    AbstractRoomState firstState;

    AbstractRoomState currentState;

    [SerializeField]
    AbstractRoomState disconnected;

    public void AddString(string str)
    {
        allLogs.Add(str);
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
        SwitchState(firstState);
        if (currentState != null)
            currentState.NumberPlayersChanged();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (currentState != null)
            currentState.NumberPlayersChanged();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (currentState != null)
        {
            currentState.NumberPlayersChanged();
        }      
    }

    public override void OnLeftRoom()
    {
        if (currentState != null)
        {
            currentState.NumberPlayersChanged();
        }
    }

    public void SwitchState(AbstractRoomState roomState)
    {
        if (currentState != null)
            currentState.Uninit();

        currentState = roomState;
        if (currentState != null)
        {
            currentState.Init();
        }
    }

    public void NextState()
    {
        if (currentState != null)
        {
            SwitchState(currentState.GetState());
        }
    }
}
