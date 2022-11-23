using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Клас який активує подію при натиску по з'єднанню в редакторі
public class ConnectionWireRespresentation : MonoBehaviour
{
	public delegate void OnPressed();
	public event OnPressed onPressed;
	public void OnClick()
	{
		//при натиску активуємо подію
		onPressed.Invoke();
		//знаходимо всі мінусові вузли та оновлюємо іх з'єднання
		var objects = FindObjectsOfType<InputWireNode>();
		foreach (var object_ in objects)
		{
			object_.UpdateLines();
		}
	}
}
