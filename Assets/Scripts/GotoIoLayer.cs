using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoIoLayer : MonoBehaviour
{
	void Awake()
	{
		transform.SetParent(FindObjectOfType<GridLayerInfo>().IOLayer.transform);
	}
}
