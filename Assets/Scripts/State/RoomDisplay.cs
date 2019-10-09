using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class RoomDisplay : AbstractRoomState
{
    List<RoomUnit> allObjects = new List<RoomUnit>();

    [SerializeField]
    RoomUnit ru;

    [SerializeField]
    Transform target;

    [SerializeField]
    Button startButton;

    public void ComputeDisplay()
    {
        Clean();
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            RoomUnit unit = Instantiate(ru,target);
            allObjects.Add(unit);
            unit.SetText(player.NickName);
        }
    }

    public override void Uninit()
    {
        base.Uninit();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public void Clean()
    {
        foreach (RoomUnit room in allObjects)
        {
            Destroy(room.gameObject);
        }
        allObjects.Clear();
    }

    public override void NumberPlayersChanged()
    {
        ComputeDisplay();
        //startButton.interactable = (PhotonNetwork.CurrentRoom.PlayerCount > 1);
    }
}
