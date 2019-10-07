using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUnit : MonoBehaviour
{
    [SerializeField]
    Text txt;

    public void SetText(string text)
    {
        txt.text = text;
    }
}
