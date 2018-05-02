using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SeekBar : MonoBehaviour {
	public HTC.UnityPlugin.Multimedia.ViveMediaDecoder decoder;
	public RectTransform fill, handle, canvas, sliderTop, sliderBottom;
	public GameObject time;
	private int lastSeekTime;
	public Text timeText;
	void Update()
	{
		float videoTime = 0;
		string currentTimeText = "";
		if ((int)decoder.getDecoderState() > 1)
		{
			videoTime = decoder.getVideoCurrentTime() / decoder.audioTotalTime;
			if (float.IsInfinity(videoTime)) videoTime = 0;
			fill.localScale = new Vector3(videoTime, 1f, 1f);
			handle.anchoredPosition3D = new Vector3(videoTime * canvas.sizeDelta.x, 32, 0);
		} else
		{
			Cursor.visible = true;
			time.SetActive(false);
			return;
		}
		Vector3 mousePos = Input.mousePosition;
		bool onSlider = false;
		onSlider = (mousePos.y > sliderBottom.position.y && mousePos.y < sliderTop.position.y);
		if (mousePos.x < 0 || mousePos.x > Screen.width||mousePos.y<0) onSlider = false;
		time.SetActive(onSlider);
		if (onSlider)
		{
			Vector3 temp = ((RectTransform)time.transform).position;
			temp.x = mousePos.x;
			if (temp.x < (((RectTransform)time.transform).sizeDelta.x*0.5f))
			{
				temp.x = (((RectTransform)time.transform).sizeDelta.x * 0.5f);
			}
			if (temp.x > Screen.width - (((RectTransform)time.transform).sizeDelta.x * 0.5f))
			{
				temp.x = Screen.width - (((RectTransform)time.transform).sizeDelta.x * 0.5f);
			}
			((RectTransform)time.transform).position = temp;
			int hoverTime = Mathf.RoundToInt(decoder.audioTotalTime * ((mousePos.x * 1f) / Screen.width));
			if (Input.GetKey(KeyCode.Mouse0))
			{
				if (hoverTime != lastSeekTime) decoder.setSeekTime(hoverTime);
				lastSeekTime = hoverTime;
			}
			if (Input.GetKeyUp(KeyCode.Mouse0)) lastSeekTime = -1;
			System.TimeSpan timespan = new System.TimeSpan(0, 0, 0, hoverTime, 0);
			if (decoder.audioTotalTime > 3600)
			{
				currentTimeText += timespan.Hours.ToString("D2") + ":";
			}
			if (decoder.audioTotalTime > 60)
			{
				currentTimeText += timespan.Minutes.ToString("D2") + ":";
			}
			currentTimeText += timespan.Seconds.ToString("D2");
			timeText.text = currentTimeText;
		}
	}
}
