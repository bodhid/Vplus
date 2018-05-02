using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonitorInterpolation : MonoBehaviour {
	public FrameInterpolation frameInterpolation;
	public RectTransform fill;
	public Text text;
	public RawImage col;
	void Update ()
	{
		fill.sizeDelta = new Vector2((float)((frameInterpolation.displayPosition + 2f) * 0.5f * 512f), 16);
		text.text = ((frameInterpolation.displayPosition + 2)*100).ToString("F0");
		col.color = (frameInterpolation.resync == 0) ? Color.white : Color.red;
	}
}
