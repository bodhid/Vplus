using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CopyOutput : MonoBehaviour {
	public RawImage target;
	public RectTransform canvas;
	private HTC.UnityPlugin.Multimedia.ViveMediaDecoder decoder;
	private void Awake()
	{
		decoder = GetComponent<HTC.UnityPlugin.Multimedia.ViveMediaDecoder>();
	}
	void Update ()
	{
		//Debug.Log(decoder.getDecoderState());
		if ((int)decoder.getDecoderState() < 2)
		{
			Notification.instance.Show("Press H for Help");
		}
		ReScale();
		if ((int)decoder.getDecoderState()==3&& FrameInterpolation.enableMotionInterpolation)
		{
			FrameInterpolation.instance.source = decoder.output;
			target.texture = FrameInterpolation.instance.output;
		}
		else
		{
			target.texture = decoder.output;
		}
		}

	private void ReScale()
	{
		RectTransform t = (RectTransform)target.GetComponent<Transform>();
		float canvasAspect = canvas.sizeDelta.x / canvas.sizeDelta.y;
		RenderTexture output = decoder.output;
		float outputAspect = (output.width * 1f) / (output.height * 1f);

		if (canvasAspect > outputAspect)
		{
			float h = Screen.height;
			float w = h * outputAspect;
			t.sizeDelta = new Vector2(w, h);
		}
		else
		{
			float w = Screen.width;
			float h = w / outputAspect;
			t.sizeDelta = new Vector2(w, h);
		}

	}
}
