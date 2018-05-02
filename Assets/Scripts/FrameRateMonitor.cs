using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateMonitor : MonoBehaviour {
	private LineRenderer line;
	public List<int> fps;
	Vector3[] positions;
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		fps = new List<int>();
		for (int i = 0; i < 512; ++i)
		{
			fps.Add(60);
		}
		positions = new Vector3[512];
		}
	void Update()
	{
		fps.Insert(0, Mathf.CeilToInt(1f / Time.deltaTime));
		fps.RemoveAt(512);
		for (int i = 0; i < 512; ++i)
		{
			positions[i] = new Vector3(i, fps[i], 0);
		}
		line.SetPositions(positions);
	}
}
