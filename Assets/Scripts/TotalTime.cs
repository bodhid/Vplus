using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTC.UnityPlugin.Multimedia;
public class TotalTime : MonoBehaviour {
	public ViveMediaDecoder decoder;
	public Text text;

	public void UpdateTimer()
	{
		if ((int)decoder.getDecoderState() > 1)
		{
			text.text= sec2String((int)decoder.getVideoCurrentTime())+" / "+ sec2String((int)decoder.audioTotalTime);
		}
	}

	private string sec2String(int s)
	{
		string result = "";
		System.TimeSpan timespan = new System.TimeSpan(0, 0, 0, s, 0);
		if (decoder.audioTotalTime > 3600)
		{
			result += timespan.Hours.ToString("D2") + ":";
		}
		if (decoder.audioTotalTime > 60)
		{
			result += timespan.Minutes.ToString("D2") + ":";
		}
		result += timespan.Seconds.ToString("D2");
		return result;
	}
	}
