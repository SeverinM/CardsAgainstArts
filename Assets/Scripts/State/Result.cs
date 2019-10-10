using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Linq;
using DG.Tweening;

public class Result : AbstractRoomState
{
    [SerializeField]
    AbstractRoomState disconnect;

    List<RoomUnit> units = new List<RoomUnit>();

    [SerializeField]
    RoomUnit ru;

    [SerializeField]
    Transform target;

    [SerializeField]
    ImageDataBank databank;

    [SerializeField]
    Image img;

    [SerializeField]
    AspectRatioFitter fitter;

    [SerializeField]
    Text txt;

    [SerializeField]
    string won;

    [SerializeField]
    string lost;

    Sequence seq;

    public override void Uninit()
    {
        base.Uninit();
        foreach(RoomUnit ru in units)
        {
            Destroy(ru.gameObject);
        }
        units.Clear();
        if(seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }

    public override void Init()
    {
        base.Init();
        float ratio = 0;
        img.sprite = databank.Sample((int)PhotonNetwork.CurrentRoom.CustomProperties["imageId"], ref ratio);
        fitter.aspectRatio = ratio;

        foreach (string str in Manager.GetInstance().choices.Keys)
        {
            RoomUnit roomu = Instantiate(ru, target);
            roomu.SetText(str);
            units.Add(roomu);
        }

        if (Manager.GetInstance().IsDeciding)
        {
            txt.text = "";
            return;
        }

        if (Manager.GetInstance().wasRight)
        {
            txt.text = won;
            txt.color = Color.green;
            seq = Manager.GetInstance().Anim.ColorChangeForWinnerAnim(txt, Color.green, Color.blue);
        }
        else
        {
            txt.text = lost;
            txt.color = Color.red;
            Manager.GetInstance().Anim.ColorChangeForLoserAnim(txt);
        }
    }

    public void SetChosenPhrases(string str)
    {
        foreach(RoomUnit room in units)
        {
            Manager.GetInstance().stateHolder.AddString("=====" + room.GetText());
            if (room.GetText() == str)
                room.GetComponent<Text>().color = Color.magenta;
        }
    }

    public override void NumberPlayersChanged()
    {
        Manager.GetInstance().stateHolder.SwitchState(disconnect);
    }
}
