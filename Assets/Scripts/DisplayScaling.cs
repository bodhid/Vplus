using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayScaling : MonoBehaviour {
	private CanvasScaler scaler;
	void Start () {
		scaler = GetComponent<CanvasScaler>();
		Debug.Log(Screen.dpi);
		//System.IO.File.WriteAllText(Application.dataPath + "/../" + "dpi.txt", Screen.dpi.ToString());
		scaler.scaleFactor = Screen.dpi / 96f;
	}
	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			scaler.scaleFactor =Mathf.Clamp( scaler.scaleFactor + Input.GetAxis("Mouse ScrollWheel"),0.5f,4f);
		}
		}
}
