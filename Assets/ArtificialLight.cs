using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Клас який контролює штучне світло
public class ArtificialLight : MonoBehaviour
{
	//При вході/виході в коллайдер штучного світла перемикаємо стан штучного освітлення на необхідний
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
