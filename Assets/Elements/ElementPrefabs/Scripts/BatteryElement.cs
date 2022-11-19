using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryElement : ElectricElement
{
	public override bool IsElementPassSignal(){return true;}

	public override void SetElementActive(bool b) { }

	private void Update()
	{
		//для кожного з'єднання викликаємо функцію рекурсивного оновлення
		foreach(var element in _outputs)
		{
			element.UpdateRecursive();
		}
	}
}
