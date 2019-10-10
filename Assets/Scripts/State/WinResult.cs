﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Linq;
using DG.Tweening;

public class WinResult : AbstractRoomState
{
    [SerializeField]
    Text txt;

    [SerializeField]
    Text txtCountdown;

    [SerializeField]
    string won;

    [SerializeField]
    string lost;

    [SerializeField]
    AbstractRoomState disconnect;

    Sequence seq;

    public override void Init()
    {
        base.Init();
        StartCoroutine(DelayedRestart(10));

        if (Manager.GetInstance().IsDeciding)
            return;

        if (Manager.GetInstance().wasRight)
        {
            Manager.GetInstance().Point++;
            txt.text = won;
            seq = Manager.GetInstance().Anim.ColorChangeForWinnerAnim(txt, Color.green, Color.white);
        }
        else
        {
            txt.text = lost;
            Manager.GetInstance().Anim.ColorChangeForLoserAnim(txt);
        }  
    }

    public override void Uninit()
    {
        base.Uninit();
        if (seq != null)
        {
            seq.Kill();
            seq = null;
        }
    }

    IEnumerator DelayedRestart(int second)
    {
        for(; second > 0; second--)
        {
            txtCountdown.text = "Next round in " + second;
            yield return new WaitForSeconds(1);
        }
        
        Manager.GetInstance().StartAgain();
    }

    public override void NumberPlayersChanged()
    {
        Manager.GetInstance().stateHolder.SwitchState(disconnect);
    }
}