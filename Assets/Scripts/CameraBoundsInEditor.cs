using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBoundsInEditor : MonoBehaviour
{
	CameraMovement cam => Camera.main.GetComponent<CameraMovement>();
    void Update()
    {
		var t = 0.05f;
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
	}
}
