using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Multimedia;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class Loader : MonoBehaviour
{
	public ViveMediaDecoder decoder;
	public SubtitleManager subs;
	string testfile = "";
	//add or try more?
	public static string[] acceptedFileNames = new string[] { "mp4", "mkv", "webm", "mov", "flv", "avi", "mp3" };
	public List<string> subtitles;
	public List<string> subtitleNames;
	public static string customUrl = null;
	//public MotionInterpolation motionInterpolation;
	public Camera cam;
	// Use this for initialization
	

	private void Awake()
	{
#if UNITY_EDITOR
		string filename = testfile;
#else
		string filename=null;
#endif


		subtitles = new List<string>();
		subtitleNames = new List<string>();
		string[] env = System.Environment.GetCommandLineArgs();
		foreach (string s in env)
		{
			foreach (string a in acceptedFileNames)
			{
				if (s.EndsWith(a))
				{
					filename = s;
					break;
				}
			}
			if (s.EndsWith("srt"))
			{
				subtitleNames.Add(new FileInfo(s).Name);
				subtitles.Add(s);
			}
		}
		if (customUrl != null) filename = customUrl;
		UnityEngine.Debug.Log("Override url: " + customUrl);
		if (filename == null) return;
		LookForMoreSubs(filename);
		//double framerate = VideoInfo.GetVideoFrameRate(filename);
		double framerate = 23.97d;
		FrameInterpolation.instance.Initialize(framerate);
		decoder.mediaPath = filename;
		decoder.initDecoder(filename, false);
		decoder.onInitComplete = new UnityEngine.Events.UnityEvent();
		decoder.onInitComplete.AddListener(delegate
		{
			decoder.startDecoding();
		});
		subtitles.Add(null);
		subtitleNames.Add("No Captions");
		subs.subtitles = subtitles;
		subs.subtitleNames = subtitleNames;
		if (subs.subtitles.Count > 0) subs.Load();
	}
	public void OnInitComplete()
	{
		decoder.startDecoding();
	}
	void LookForMoreSubs(string filename)
	{
		try //fails with http url
		{
			FileInfo f = new FileInfo(filename);
			DirectoryInfo d = f.Directory;
			AddSubsFromDirectory(d);
			foreach (DirectoryInfo dd in d.GetDirectories())
			{
				AddSubsFromDirectory(dd);
			}
		}
		catch (System.Exception e)
		{
			UnityEngine.Debug.LogWarning(e.Message + " - " + e.StackTrace);
		}
	}
	void AddSubsFromDirectory(DirectoryInfo d)
	{
		foreach (FileInfo ff in d.GetFiles())
		{
			if (ff.FullName.EndsWith("srt"))
			{
				UnityEngine.Debug.Log("Subs found: " + ff.FullName);
				subtitleNames.Add(ff.Name);
				subtitles.Add(ff.FullName);
			}
		}
	}
}
