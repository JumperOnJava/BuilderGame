using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���� ���� ��������� ������ �����
public class ArtificialLight : MonoBehaviour
{

	//��� ����/����� � ��������� �������� ����� ���������� ���� �������� ���������
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log($"Trying to activate {collision.gameObject.name} from {name}");
		if (collision.gameObject.TryGetComponent<LightSensorElement>(out var light))
		{
			Debug.Log("Successfull a");
			light.ArtificalLight++;
		}
		else
		{
			Debug.Log("Unsuccessfull a");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Debug.Log($"Trying to deactivate {collision.gameObject.name} from {name}");

		if (collision.gameObject.TryGetComponent<LightSensorElement>(out var light))
		{
			Debug.Log("Successfull d");
			light.ArtificalLight--;
		}
		else
		{
			Debug.Log("Unsuccessfull d");
		}
	}
}
