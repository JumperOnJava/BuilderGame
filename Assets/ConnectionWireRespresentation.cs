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
		Debug.Log("clicked on wire");
		onPressed.Invoke();
	}
}
