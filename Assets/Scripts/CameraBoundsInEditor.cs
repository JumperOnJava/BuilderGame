using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBoundsInEditor : MonoBehaviour
{
	CameraMovement cam => Camera.main.GetComponent<CameraMovement>();
	static float t = 0.05f;

	void Update()
	{
		var _startPath = cam._startPath;
		for (int i = 0; i < _startPath.Count-1; i++)
		{
			var c = (float)i / (_startPath.Count);
			Debug.DrawLine(_startPath[i], _startPath[i + 1], new Color(1 - c, c, 0),t);
		}
		Vector3 v1, v2;
		v1 = new Vector3(cam._leftClamp, cam._upClamp);
		v2 = new Vector3(cam._rightClamp, cam._upClamp);
		Debug.DrawLine(v1, v2, Color.red, t);
		v1 = new Vector3(cam._leftClamp, cam._downClamp);
		v2 = new Vector3(cam._rightClamp, cam._downClamp);
		Debug.DrawLine(v1, v2, Color.red, t);
		v1 = new Vector3(cam._rightClamp, cam._downClamp);
		v2 = new Vector3(cam._rightClamp, cam._upClamp);
		Debug.DrawLine(v1, v2, Color.red, t);
		v1 = new Vector3(cam._leftClamp, cam._upClamp);
		v2 = new Vector3(cam._leftClamp, cam._downClamp);
		Debug.DrawLine(v1, v2, Color.red, t);
		for (float i = 0; i < Mathf.PI * 2; i += 0.3f)
		{
			Debug.DrawLine(cam._targetPos + new Vector3(Mathf.Sin(i - 0.3f), Mathf.Cos(i - 0.3f)) / 2, cam._targetPos + new Vector3(Mathf.Sin(i), Mathf.Cos(i)) / 2, Color.green, t);
		}
	}
	public static void DebugPoint(Vector3 pos,float size)
	{
		for (float i = 0; i < Mathf.PI * 2; i += 0.3f)
		{
			Debug.DrawLine(pos + new Vector3(Mathf.Sin(i - 0.3f), Mathf.Cos(i - 0.3f)) *size, pos + new Vector3(Mathf.Sin(i), Mathf.Cos(i)) *size, Color.green, t);
		}
	}
}
