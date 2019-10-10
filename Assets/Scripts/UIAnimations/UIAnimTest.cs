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
		scalingUpDown.Append(transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f));
		scalingUpDown.Append(transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f));
		scalingUpDown.Append(transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f));
	}

	public void ColorChangeForWinnerAnim(Text text, Color firstColor, Color secondColor) {
		text.color = secondColor;
		Sequence colorChange = DOTween.Sequence();
		colorChange.Append(text.DOColor(firstColor, 1.5f));
		colorChange.Append(text.DOColor(secondColor, 1.5f));
		colorChange.SetLoops(-1, LoopType.Yoyo);
	}

	public void ColorChangeForLoserAnim(Text text) {
		Color startingColor = text.color;
		text.DOColor(new Color(startingColor.r, startingColor.g, startingColor.b, 0.5f),1f);
	}

	/*public void ArriveFromLeftAnim(Transform transform) {
		Sequence arrivingLeft = DOTween.Sequence();
		arrivingLeft.Append(transform.DOMoveX(190, 1f));
		arrivingLeft.Append(transform.DOMoveX(10, 0.2f));
		arrivingLeft.Append(transform.DOMoveX(190, 0.2f));
	}*/
}
