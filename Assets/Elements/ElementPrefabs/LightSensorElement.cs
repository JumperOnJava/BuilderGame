using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensorElement : SensorElement
{
	[SerializeField]
	bool active;
	public override bool IsElementPassSignal()
	{
		return active;
	}

}
