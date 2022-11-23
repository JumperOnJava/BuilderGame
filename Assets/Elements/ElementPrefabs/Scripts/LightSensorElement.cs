using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Клас який контролює фоторезистор
/// </summary>
public class LightSensorElement : SensorElement
{
	public int ArtificalLight = 0;
	public override bool IsElementPassSignal()
	{
		//перевіряємо чи є над фоторезистором будь який об'єкт
		RaycastHit2D skyCheck = Physics2D.Raycast(transform.position, Vector2.up);

		//Якщо над фоторезистором немає ніяких об'єктів або його освітлює штучне світло то проводимо сигнал

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
