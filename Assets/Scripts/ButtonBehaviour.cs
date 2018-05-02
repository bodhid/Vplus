using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTC.UnityPlugin.Multimedia;
public class ButtonBehaviour : MonoBehaviour
{
	public RawImage pauseResume;
	public Button loop;
	public Texture playTexture, pauseTexture;
	public ViveMediaDecoder decoder;
	public RectTransform seekbarTop;
	private float lastMouseClick=-1;
	private float volumeBeforeMute=1;
	public SubtitleManager subs;
	public GameObject Help, Url,Debug;
	public InputField urlText;
	void Update()
	{
		if (decoder.getDecoderState() == ViveMediaDecoder.DecoderState.PAUSE || decoder.getDecoderState() == ViveMediaDecoder.DecoderState.EOF)
		{
			pauseResume.texture = playTexture;
		}
		else
		{
			pauseResume.texture = pauseTexture;
		}
		ColorBlock current = loop.colors;
		current.normalColor = decoder.loop ? Color.black : Color.gray;
		current.highlightedColor = decoder.loop ? Color.black : Color.gray;
		loop.colors = current;

		if (Input.mousePosition.y > seekbarTop.position.y)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (Time.time < lastMouseClick + 0.5) Fullscreen();
				lastMouseClick = Time.time;
			}
		}
		if (Input.GetKeyDown(KeyCode.F)||XInput.GetButtonDown(0,XInput.Button.X))
		{
			Fullscreen();
			Notification.instance.Show(Notification.NotificationType.Fullscreen);
		}
		if (Input.GetKeyDown(KeyCode.C)||XInput.GetButtonDown(0,XInput.Button.Y))
		{
			subs.currentSub++;
			subs.Load();
			Notification.instance.Show(subs.subtitleNames[subs.currentSub]);
		}
		if (Input.GetKeyDown(KeyCode.L)|| XInput.GetButtonDown(0, XInput.Button.B))
		{
			SetLoop();
			Notification.instance.Show(Notification.NotificationType.Loop);
		}
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.D))
		{
			Debug.SetActive(Debug.activeSelf ? false : true);
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			Url.SetActive(!Url.activeSelf);
		}
		if (Input.GetKeyDown(KeyCode.O)||XInput.GetButtonDown(0,XInput.Button.Start))
		{
			string start = null;
			try //get current file directory if exists
			{
				System.IO.FileInfo f = new System.IO.FileInfo(decoder.mediaPath);
				if (f.Exists)
				{
					System.IO.DirectoryInfo d = f.Directory;
					start = d.FullName;
				}
			}
			catch (System.Exception)
			{
				//can go wrong in many ways
			}
			Cursor.visible = true;
			SFB.ExtensionFilter[] filter = new SFB.ExtensionFilter[] { new SFB.ExtensionFilter("Supported Files", Loader.acceptedFileNames) };
			string[] file = SFB.StandaloneFileBrowser.OpenFilePanel("Select File", start, filter, false);
			if (file.Length>0)
			{
				Loader.customUrl = file[0];
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}
		Help.SetActive(Input.GetKey(KeyCode.H));
		if (Input.GetAxis("Mouse ScrollWheel") != 0||XInput.GetAxis(0,XInput.Axis.RY)!=0)
		{
			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
			{
				float volumeJump = 0.1f;
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) volumeJump = 1f;
				decoder.setVolume(Mathf.Clamp01(decoder.getVolume() + ((Input.GetAxis("Mouse ScrollWheel")+ (XInput.GetAxis(0, XInput.Axis.RY)*0.1f)) * volumeJump)));
				Notification.instance.Show("♫ " + Mathf.RoundToInt(decoder.getVolume() * 100f).ToString());
			}
		}
		if (Input.GetKeyDown(KeyCode.M)||XInput.GetButtonDown(0,XInput.Button.RS))
		{
			if (decoder.getVolume() == 0)
			{
				decoder.setVolume(volumeBeforeMute);
			}
			else
			{
				volumeBeforeMute = decoder.getVolume();
				decoder.setVolume(0);
			}
			Notification.instance.Show("♫ " + Mathf.RoundToInt(decoder.getVolume() * 100f).ToString());
		}

		if (Input.GetKeyDown(KeyCode.Space)||XInput.GetButtonDown(0,XInput.Button.A))
		{
			if (decoder.getDecoderState() == ViveMediaDecoder.DecoderState.PAUSE)
			{
				Notification.instance.Show(Notification.NotificationType.Play);
			}
			else
			{
				Notification.instance.Show(Notification.NotificationType.Pause);
			}
			PauseResume();
			
		}
		float jump = 20;
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) jump = 10;
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) jump = 60;
		if (Input.GetKeyDown(KeyCode.RightArrow)||XInput.GetButtonDown(0,XInput.Button.DPAD_Right))
		{
			Notification.instance.Show(Notification.NotificationType.Forward);
			Forward(jump);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) || XInput.GetButtonDown(0, XInput.Button.DPAD_Left))
		{
			Notification.instance.Show(Notification.NotificationType.Backward);
			Backwards(jump);
		}

	}
	public void PauseResume()
	{
		if (decoder.getDecoderState() == ViveMediaDecoder.DecoderState.PAUSE)
		{
			decoder.setResume();
		}
		else
		{
			decoder.setPause();
		}
		if (decoder.getDecoderState() == ViveMediaDecoder.DecoderState.EOF)
		{
			decoder.setResume();
			decoder.setSeekTime(0);
		}
	}
	public void SetLoop()
	{
		decoder.loop = !decoder.loop;
	}
	public void Backwards(float jump=10f)
	{
		decoder.setSeekTime(decoder.getVideoCurrentTime() - jump);
	}
	public void Forward(float jump = 10f)
	{
		decoder.setSeekTime(Mathf.Clamp(decoder.getVideoCurrentTime() + jump, 0, decoder.audioTotalTime));
	}
	public void Fullscreen()
	{
		Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
		if (Screen.fullScreen)
		{
			Screen.SetResolution(decoder.output.width,decoder.output.height, false);
		}
		else
		{
			Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
		}
	}
	public void LoadCustomURL()
	{
		Loader.customUrl = urlText.text;
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
	}
