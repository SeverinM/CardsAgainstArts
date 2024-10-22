﻿using System.Collections;
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

    [SerializeField]
    AspectRatioFitter fitter;

    [SerializeField]
    Button btn;

    [SerializeField]
    Text txt;

    public override void Uninit()
    {
        dontChose.SetActive(true);
        chose.SetActive(true);
        btn.interactable = true;
        txt.text = "";
        base.Uninit();
    }

    public void Sent()
    {
        btn.interactable = false;
        txt.text = "Sent! Waiting for other players";
    }

    public override void NumberPlayersChanged()
    {
        Manager.GetInstance().stateHolder.SwitchState(disconnect);
    }

    public override void Init()
    {
        base.Init();
        Manager.GetInstance().choices.Clear();
        if (Manager.GetInstance().IsDeciding)
            dontChose.SetActive(false);
        else
            chose.SetActive(false);

        Manager.GetInstance().stateHolder.AddString(PhotonNetwork.CurrentRoom.CustomProperties["imageId"].ToString());
        imgs.Sample((int)PhotonNetwork.CurrentRoom.CustomProperties["imageId"]);
        float ratio = imgs.ratio;
        img.sprite = imgs.sprt;
        fitter.aspectRatio = ratio;
    }
}
