﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CreditState : AbstractRoomState
{
    [SerializeField]
    float secs;

    public override void Init()
    {
        base.Init();
        StartCoroutine(NextState());
    }

    IEnumerator NextState()
    {
        yield return new WaitForSeconds(secs);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
