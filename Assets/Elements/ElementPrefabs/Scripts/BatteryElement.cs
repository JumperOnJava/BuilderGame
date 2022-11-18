using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryElement : ElectricElement
{
	public override bool IsElementPassSignal()
	{
		return true;
	}

	public override void SetElementActive(bool b) { }

	public override bool UpdateRecursive(bool input,int index,BatteryElement battery)
	{
		//Debug.Log($"Battery Recieved {index}:{input}");
		if (index == 0)
			throw new System.Exception("Short circuit");
		if(battery==this)
		return input && index > 0 && battery == this;
		else return base.UpdateRecursive(input,index,battery);
	}
	private void Update()
	{
		//для кожного з'єднання викликаємо функцію рекурсивного оновлення
		foreach(var element in _outputs)
		{
			try
			{
				element.UpdateRecursive(true, 0, this);
			}
			catch
			{

			}
		}
	}
}
