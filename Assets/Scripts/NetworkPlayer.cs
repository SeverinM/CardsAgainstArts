using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField]
    AbstractRoomState announce;

    [PunRPC]
    public void NewRound(bool newValue)
    {
        Manager.GetInstance().IsDeciding = newValue;
        Manager.GetInstance().stateHolder.SwitchState(announce);
    }
}
