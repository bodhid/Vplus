using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class SubtitleManager : MonoBehaviour
{
	public List<string> subtitles;
	public List<string> subtitleNames;
	public HTC.UnityPlugin.Multimedia.ViveMediaDecoder decoder;
	public int currentSub = 0;
	private List<SubtitleBlock> subs;
	public Text[] textObjects;
	public RectTransform videoOutput;
	[System.Serializable]
	public class SubtitleBlock
	{
		public SubtitleBlock(string _text, int _startTimeMS, int _endTimeMS)
		{
			text = _text;
			startTimeMS = _startTimeMS;
			endTimeMS = _endTimeMS;
		}
		public string text;
		public int startTimeMS, endTimeMS;
	}

	private void Update()
	{
		Vector2 size = ((RectTransform)transform).sizeDelta;
		size.y = ((RectTransform)videoOutput.transform).sizeDelta.y / 4f;
		((RectTransform)transform).sizeDelta = size;
		if ((int)decoder.getDecoderState() < 2) return;
		int currentTimeInMS = (int)(decoder.getVideoCurrentTime() * 1000);
		int block = GetCurrentBlock(currentTimeInMS);
		string text = "";
		if (block != -1) text = subs[block].text;
		for (int i = 0; i < textObjects.Length; ++i)
		{
			textObjects[i].text = text;
			textObjects[i].fontSize = (int)(size.y * 0.3f);
		}
	}

	public void Load()
	{
		if (currentSub >= subtitles.Count) currentSub = 0;
		if (subs != null) subs.Clear();
		if (subtitles[currentSub] == null)
		{
			subs = null;
			return;
		}
		subs = new List<SubtitleBlock>();
		string[] lines = File.ReadAllLines(subtitles[currentSub]);

		int currentline = 0;
		int currentBlock = 1;
		while (true)
		{
			if (lines[currentline].Trim() == currentBlock.ToString())
			{
				string timestamp = lines[currentline + 1].Trim();
				string[] stamps = timestamp.Split(new string[] { " --> " }, System.StringSplitOptions.None);
				int start = TimestampToMilliseconds(stamps[0]);
				int end = TimestampToMilliseconds(stamps[1]);
				string text = "";
				for (int j = currentline + 2; j < currentline + 8; ++j)
				{
					if (string.IsNullOrEmpty(lines[j]))
					{
						//end of subBlock
						currentline = j;
						currentBlock++;
						break;
					}
					text += lines[j] + "\n";
				}
				subs.Add(new SubtitleBlock(text.Trim(), start, end));
			}
			currentline++;
			if (currentline == lines.Length) break;
		}
		Debug.Log("subs loaded: " + subs.Count + " blocks");
		System.GC.Collect();
	}
	public int TimestampToMilliseconds(string stamp)
	{
		int hours = int.Parse(stamp.Substring(0, 2));
		int minutes = int.Parse(stamp.Substring(3, 2));
		int seconds = int.Parse(stamp.Substring(6, 2));
		int milliseconds = int.Parse(stamp.Split(","[0])[1]);
		System.TimeSpan timespan = new System.TimeSpan(0, hours, minutes, seconds, milliseconds);
		return (int)timespan.TotalMilliseconds;
	}
	public int GetCurrentBlock(int time)
	{
		if (subs == null) return -1;
		//binary search
		int result = -1;
		int currentBlock = subs.Count / 2;
		int jumpSize = subs.Count / 4;
		for (int i = 0; i < 128; ++i)
		{
			if (currentBlock <= -1 || currentBlock >= subs.Count) break;
			if (time < subs[currentBlock].startTimeMS)
			{
				currentBlock -= jumpSize;
				jumpSize = jumpSize / 2;
				if (jumpSize < 1) jumpSize = 1;
				
				continue;
			}
			if (time > subs[currentBlock].endTimeMS)
			{
				currentBlock += jumpSize;
				jumpSize = jumpSize / 2;
				if (jumpSize < 1) jumpSize = 1;
				
				continue;
			}
			result = currentBlock;
			break;
		}
		return result;
	}
}
