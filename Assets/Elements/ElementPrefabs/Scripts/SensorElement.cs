using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//клас який має наслідувати кожен датчик
public abstract class SensorElement : ElectricElement
{
	//заглушка
	public override void SetElementActive(bool b) { }

	public void Awake()
	{
		_resistance = false;
	}
}
