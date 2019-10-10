using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class AnnounceState : AbstractRoomState
{
    [SerializeField]
    protected Text txt;

    [SerializeField]
    protected float time;

    [SerializeField]
    protected string youDecide;

    [SerializeField]
    protected string youDontDecide;

    [SerializeField]
    AbstractRoomState disconnect;

    public override void Init()
    {
        base.Init();
        Manager.GetInstance().wasRight = false;
        txt.text = Manager.GetInstance().IsDeciding ? youDecide : youDontDecide;
        StartCoroutine(DelayedTransition());
    }

    public override void NumberPlayersChanged()
    {
        Manager.GetInstance().stateHolder.SwitchState(disconnect);
    }

    IEnumerator DelayedTransition()
    {
        yield return new WaitForSeconds(time);
        Manager.GetInstance().stateHolder.SwitchState(GetState());
    }
}
