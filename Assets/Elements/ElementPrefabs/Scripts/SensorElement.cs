using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SensorElement : ElectricElement
{
	public void Awake()
	{
		_resistance = false;
	}
	public override void OnElementActive() { }
}
