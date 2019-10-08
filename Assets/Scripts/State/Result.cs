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

    public void SetRightness(bool value)
    {
        if (value)
            wrong.SetActive(false);
        else
            right.SetActive(false);
    }
}
