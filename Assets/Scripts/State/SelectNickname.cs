using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class SelectNickname : AbstractRoomState
{
    [SerializeField]
    Button btn;

    public void InputChanged(InputField iF)
    {
        btn.interactable = (iF.text.Length > 0);
    }

    public void Next(InputField iF)
    {
        PhotonNetwork.NickName = iF.text;
        PhotonNetwork.ConnectUsingSettings();
        btn.interactable = false;
        iF.interactable = false;
    }
}
