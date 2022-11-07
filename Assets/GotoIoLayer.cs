using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoIoLayer : MonoBehaviour
{
	// Start is called before the first frame update
	void Awake()
	{
		transform.SetParent(FindObjectOfType<GridLayerInfo>().IOLayer.transform);
	}
}
