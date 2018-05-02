using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
public class VideoInfo
{
	[System.Serializable]
	public class InfoBlock
	{
		public Audio audio;
		public Video video;
		[System.Serializable]
		public class Video
		{
			public string codec, colorFormat;
			public int width, height;
			public double bitrate, framerate;
		}
		[System.Serializable]
		public class Audio
		{
			public string codec;
			public int channels, sampleRate;
			public double bitrate;
		}
	}
	public static double GetVideoFrameRate(string filename)
	{
		try
		{
#if UNITY_EDITOR
			string ffprobePath = Application.streamingAssetsPath + "/ffprobe.exe";
#else
			string ffprobePath = Application.dataPath + "/Plugins/ffprobe.exe";
#endif
			ffprobePath = Path.GetFullPath(ffprobePath);

			UnityEngine.Debug.Log("Video file: " + filename);
			UnityEngine.Debug.Log("ffprobe path: " + ffprobePath);

			string arguments = "-i " + "\"" + Path.GetFullPath(filename) + "\"" + " -hide_banner";
			arguments = "-v error -select_streams v:0 -show_entries stream=avg_frame_rate -of default=noprint_wrappers=1:nokey=1 " + "\"" + Path.GetFullPath(filename) + "\"";
			UnityEngine.Debug.Log("Arguments: " + arguments);

			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.FileName = ffprobePath;
			p.StartInfo.Arguments = arguments;
			p.Start();

			StreamReader or = p.StandardOutput;
			StreamReader er = p.StandardError;
			double result = -1;
			while (!or.EndOfStream)
			{
				string line = or.ReadLine();
				if (line.Contains("/"))
				{
					string[] s = line.Split("/"[0]);
					result = double.Parse(s[0]) / double.Parse(s[1]);
				}
			}
			while (!er.EndOfStream)
			{
				UnityEngine.Debug.Log("ERROR " + er.ReadLine());
			}
			return result;
		}
		catch (System.Exception e)
		{
			UnityEngine.Debug.LogError(e.Message + " - " + e.StackTrace);
			return -1;
		}
	}
}