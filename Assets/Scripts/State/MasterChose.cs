using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MasterChose : AbstractRoomState
{
    List<RoomUnit> allObjs = new List<RoomUnit>();

    [SerializeField]
    RoomUnit prefab;

    [SerializeField]
    Transform target;

    [SerializeField]
    GameObject chose;

    [SerializeField]
    GameObject dontChose;

    public override void Init()
    {
        base.Init();

        if (Manager.GetInstance().IsDeciding)
        {
            dontChose.SetActive(false);
            foreach (string txt in Manager.GetInstance().choices.Keys)
            {
                RoomUnit ru = Instantiate(prefab, target);
                ru.SetText(txt);
                ru.GetComponent<Button>().onClick.AddListener(() => Manager.GetInstance().Chose(ru.GetText()));
            }
        }
        else
        {
            chose.SetActive(false);
        }
    }
}
