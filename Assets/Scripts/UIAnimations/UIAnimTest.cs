using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void RescaleUpDownAnim(Transform transform) { 
		Sequence scalingUpDown = DOTween.Sequence();
		scalingUpDown.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.2f));
		scalingUpDown.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f));
	}

	public void ColorChangeForWinnerAnim(Text text) {
		text.color = Color.cyan;
		Sequence colorChange = DOTween.Sequence();
		colorChange.Append(text.DOColor(Color.yellow, 0.5f));
		colorChange.Append(text.DOColor(Color.cyan, 0.5f));
		colorChange.SetLoops(-1, LoopType.Yoyo);
	}

	public void ColorChangeForLoserAnim(Text text) {
		Color startingColor = text.color;
		text.DOColor(new Color(startingColor.r, startingColor.g, startingColor.b, 0.5f),0.5f);
	}
}
