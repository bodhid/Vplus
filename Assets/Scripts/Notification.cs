using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Notification : MonoBehaviour
{
	public static Notification instance;
	public RawImage image;
	public GameObject notificationText;
	public Text notificationTextText;
	public float timer = 0f;
	public float textTimer = 0f;
	public enum NotificationType
	{
		Pause = 0,
		Play = 1,
		Forward = 2,
		Backward = 3,
		Loop = 4,
		Fullscreen = 5
	}
	public Texture2D[] typeTextures;
	void Start()
	{
		instance = this;
		Show("Press H for Help");
		timer = 0;
	}
	void Update()
	{
		image.color = new Color(1, 1, 1, Mathf.Clamp01(timer));
		notificationText.SetActive(textTimer > 0);
		timer -= Time.deltaTime;
		textTimer -= Time.deltaTime;
	}
	public void Show(NotificationType nType)
	{
		image.texture = typeTextures[(int)nType];
		timer = 1;
	}
	public void Show(string text)
	{
		notificationTextText.text=text;
		textTimer = 1;
	}
}
