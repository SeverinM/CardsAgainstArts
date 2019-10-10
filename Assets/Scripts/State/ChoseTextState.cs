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
        txt.text = "Sent ! waiting for others players";
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

        float ratio = 0;
        img.sprite = imgs.Sample((int)PhotonNetwork.CurrentRoom.CustomProperties["imageId"], ref ratio);
        fitter.aspectRatio = ratio;
    }
}
