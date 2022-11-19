using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log($"TriggerEnter this {gameObject.name} other {other.gameObject.name}");
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log($"TriggerExit this {gameObject.name} other {other.gameObject.name}");
	}
}
