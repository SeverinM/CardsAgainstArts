using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class AbstractRoomState : MonoBehaviourPunCallbacks
{
    [SerializeField]
    protected List<GameObject> allGobs;

    public virtual void Init()
    {
        foreach(GameObject gob in allGobs)
        {
            gob.SetActive(true);
        }
    }

    public virtual void Uninit()
    {
        foreach (GameObject gob in allGobs)
        {
            gob.SetActive(false);
        }
    }

    public virtual void NumberPlayersChanged()
    {

    }
}
