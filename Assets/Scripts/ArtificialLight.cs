using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���� ���� ��������� ������ �����
public class ArtificialLight : MonoBehaviour
{
	//��� ����/����� � ��������� �������� ����� ���������� ���� �������� ��������� �� ����������
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.TryGetComponent<LightSensorElement>(out var light))
			light.ArtificalLight++;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent<LightSensorElement>(out var light))
			light.ArtificalLight--;
	}
}
