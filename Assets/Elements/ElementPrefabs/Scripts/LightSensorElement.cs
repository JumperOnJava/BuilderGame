using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
///  лас €кий контролюЇ фоторезистор
/// </summary>
public class LightSensorElement : SensorElement
{
	public int ArtificalLight = 0;
	public override bool IsElementPassSignal()
	{
		//перев≥р€Їмо чи Ї над фоторезистором будь €кий об'Їкт
		RaycastHit2D skyCheck = Physics2D.Raycast(transform.position, Vector2.up);

		//якщо над фоторезистором немаЇ н≥€ких об'Їкт≥в або його осв≥тлюЇ штучне св≥тло то проводимо сигнал

		if (skyCheck.collider == null || ArtificalLight > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
}
