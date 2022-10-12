using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LightSensorElement : SensorElement
{ 
	public override bool IsElementPassSignal()
	{
		bool active = false;
		RaycastHit2D skyCheck = Physics2D.Raycast(transform.position, Vector2.up);
		Debug.DrawRay(transform.position, Vector2.up*skyCheck.distance, Color.red,0.1f);
		Debug.Log($"{skyCheck.distance} == null? {skyCheck.collider==null}");
		return skyCheck.collider == null;
	}

}
