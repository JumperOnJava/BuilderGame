using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryElement : ElectricElement
{
	public override bool IsElementPassSignal()
	{
		var loop = false;
		try
		{
			loop=CheckLoop(0);
		}
		catch
		{
			loop = false;
		}
		Debug.Log(loop);
		return loop;
	}

	public override void OnElementActive(){}

	public override bool UpdateRecursive(bool input,int index)
	{
		Debug.Log($"Battery Recieved {index}:{input}");
		return input && index > 0;
	}
	private void Update()
	{
		foreach(var element in _outputs)
		{
			element.UpdateRecursive(true, 0);
		}
	}
}
