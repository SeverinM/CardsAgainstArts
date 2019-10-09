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

    [SerializeField]
    AbstractRoomState disconnect;

    [SerializeField]
    ImageDataBank databank;

    [SerializeField]
    Image img;

    public override void Uninit()
    {
        foreach(RoomUnit roomu in allObjs)
        {
            Destroy(roomu.gameObject);
        }
        allObjs.Clear();
        chose.SetActive(true);
        dontChose.SetActive(true);
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
        {
            dontChose.SetActive(false);
            img.sprite = databank.Sample((int)PhotonNetwork.CurrentRoom.CustomProperties["imageId"]);
            foreach (string txt in Manager.GetInstance().choices.Keys)
            {
                RoomUnit ru = Instantiate(prefab, target);
                ru.SetText(txt);
                ru.GetComponent<Button>().onClick.AddListener(() => Manager.GetInstance().Chose(ru.GetText()));
                allObjs.Add(ru);
            }
        }
        else
        {
            chose.SetActive(false);
        }       
    }
}
