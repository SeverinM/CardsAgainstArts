using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class RoomDisplay : MonoBehaviour
{
    [SerializeField]
    List<RoomUnit> allObjects;

    [SerializeField]
    RoomUnit ru;

    public void ComputeDisplay()
    {
        Clean();
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            RoomUnit unit = Instantiate(ru,transform);
            allObjects.Add(unit);
            unit.SetText(player.NickName);
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
}
