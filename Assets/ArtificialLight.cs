using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArtificialLight : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		collision.otherCollider.gameObject.GetComponent<LightSensorElement>().isInArtificalLight = true;
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		collision.otherCollider.gameObject.GetComponent<LightSensorElement>().isInArtificalLight = false;
	}
}
