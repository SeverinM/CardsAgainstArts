using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Result : AbstractRoomState
{
    [SerializeField]
    GameObject right;

    [SerializeField]
    GameObject wrong;

    [SerializeField]
    AbstractRoomState disconnect;

    public override void Uninit()
    {
        base.Uninit();
        wrong.SetActive(true);
        right.SetActive(true);
    }

    public override void NumberPlayersChanged()
    {
        Manager.GetInstance().stateHolder.SwitchState(disconnect);
    }

    public void SetRightness(bool value)
    {
        if (value)
            wrong.SetActive(false);
        else
            right.SetActive(false);
    }
}
