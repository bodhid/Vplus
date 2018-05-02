using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShow : MonoBehaviour {
	public float HideTimer = 0;
	private RectTransform t;
	public RectTransform seekTop;
	Vector3 lastMouse;
	public TotalTime totalTime;
	public HTC.UnityPlugin.Multimedia.ViveMediaDecoder decoder;
	// Use this for initialization
	void Start () {
		t = (RectTransform)transform;
		t.anchoredPosition3D = new Vector3(0, -64, 0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 posGoto = (HideTimer < 0) ? new Vector3(0, -64, 0) : Vector3.zero;
		Cursor.visible = (HideTimer > 0||((int)decoder.getDecoderState()<2));
		t.anchoredPosition3D = Vector3.Lerp(t.anchoredPosition3D, posGoto, Time.deltaTime * 8f);
		Vector3 newMouse = Input.mousePosition;
		if (Input.GetAxis("Mouse X")!=0&&Input.GetAxis("Mouse Y")!=0||newMouse.y< seekTop.position.y)
		{
			if (newMouse.x > 0 && newMouse.y > 0 && newMouse.x < Screen.width && newMouse.y < Screen.height)
			{
				if ((int)decoder.getDecoderState() > 1)
				{
					HideTimer = 3f;
				}
			}
		}
		if (HideTimer > 0)
		{
			totalTime.UpdateTimer(); // only update when visible since it generates garbage
		}
		HideTimer -= Time.deltaTime;
	}
}
