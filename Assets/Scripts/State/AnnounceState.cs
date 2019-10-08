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

    public override void Init()
    {
        base.Init();
        txt.text = Manager.GetInstance().IsDeciding ? youDecide : youDontDecide;
        StartCoroutine(DelayedTransition());
    }

    IEnumerator DelayedTransition()
    {
        yield return new WaitForSeconds(time);
        Manager.GetInstance().stateHolder.SwitchState(GetState());
    }
}
