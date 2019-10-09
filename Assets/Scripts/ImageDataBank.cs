using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataBank : MonoBehaviour
{
    [SerializeField]
    List<Sprite> allSprites;

    public Sprite Sample(int value)
    {
        return allSprites[value % allSprites.Count];
    }
}
