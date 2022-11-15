using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// ���� ���� ��������� ������������
/// </summary>
public class LightSensorElement : SensorElement
{
	public bool isInArtificalLight = false;
	public override bool IsElementPassSignal()
	{
		//���������� �� � ��� �������������� ���� ���� ��'���
		RaycastHit2D skyCheck = Physics2D.Raycast(transform.position, Vector2.up);
		
		//���� ��� �������������� ���� ����� ��'���� ��� ���� ������� ������ ����� �� ��������� ������
		return skyCheck.collider == null || isInArtificalLight;
	}
	
}
