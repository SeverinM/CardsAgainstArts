using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class DisconnectState : AbstractRoomState
{
    public override void Init()
    {
        base.Init();
        Manager.GetInstance().disableEvents = true;
    }
}
