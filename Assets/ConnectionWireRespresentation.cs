using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionWireRespresentation : MonoBehaviour
{
	public delegate void OnPressed();
	public event OnPressed onPressed;
	public void OnClick()
	{
		onPressed.Invoke();

		var objects = FindObjectsOfType<InputWireNode>();
		foreach (var object_ in objects)
		{
			object_.UpdateLines();
		}


		Debug.Log("clicked on wire");
	}
}
