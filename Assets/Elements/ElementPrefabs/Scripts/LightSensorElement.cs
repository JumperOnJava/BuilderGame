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
	public int ArtificalLight = 0;
	public override bool IsElementPassSignal()
	{
		//���������� �� � ��� �������������� ���� ���� ��'���
		RaycastHit2D skyCheck = Physics2D.Raycast(transform.position, Vector2.up);

		//���� ��� �������������� ���� ����� ��'���� ��� ���� ������� ������ ����� �� ��������� ������

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
