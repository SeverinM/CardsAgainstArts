using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ChoseTextState : AbstractRoomState
{
    [SerializeField]
    GameObject chose;

    [SerializeField]
    GameObject dontChose;

    [SerializeField]
    AbstractRoomState disconnect;

    [SerializeField]
    Image img;

    [SerializeField]
    ImageDataBank imgs;

    public override void Uninit()
    {
        dontChose.SetActive(true);
        chose.SetActive(true);
        base.Uninit();
    }

    public override void NumberPlayersChanged()
    {
        Manager.GetInstance().stateHolder.SwitchState(disconnect);
    }

    public override void Init()
    {
        base.Init();
        if (Manager.GetInstance().IsDeciding)
            dontChose.SetActive(false);
        else
            chose.SetActive(false);

        img.sprite = imgs.Sample((int)PhotonNetwork.CurrentRoom.CustomProperties["imageId"]);
    }
}
