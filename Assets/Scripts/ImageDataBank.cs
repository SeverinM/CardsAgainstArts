using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataBank : MonoBehaviour
{
    [SerializeField]
    List<Sprite> allSprites;

    public Sprite Sample(int value, ref float ratio)
    {
        Sprite sprt = allSprites[value % allSprites.Count];
        ratio = sprt.bounds.size.x / sprt.bounds.size.y;
        return sprt;
    }
}
