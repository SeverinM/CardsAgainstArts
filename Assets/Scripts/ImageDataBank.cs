using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDataBank : MonoBehaviour
{
    [SerializeField]
    List<Sprite> allSprites;
    List<Sprite> removedSprites = new List<Sprite>();

    public Sprite sprt { get; private set; }
    public float ratio { get; private set; }

    public void Sample(int value)
    {
        if (allSprites.Count == 0)
        {
            allSprites = new List<Sprite>(removedSprites);
            allSprites.Clear();
        }

        sprt = allSprites[allSprites.Count == 1 ? 0 : value % allSprites.Count];
        ratio = sprt.bounds.size.x / sprt.bounds.size.y;
        removedSprites.Add(sprt);
        allSprites.Remove(sprt);
    }
}
