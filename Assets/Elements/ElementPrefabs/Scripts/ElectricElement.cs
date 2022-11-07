using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;

public abstract class ElectricElement : GenericElement
{
	[SerializeField]
	protected List<ElectricElement> _outputs = new();
	//private delegate void OnRecievedAllInputs(bool input);
	public void AddOutput(ElectricElement output)
	{
		_outputs.Add(output);
	}
	public abstract bool IsElementPassSignal();
	public abstract void OnElementActive();

	protected bool _resistance = true;
	public virtual bool UpdateRecursive(bool stillComplete, int index, BatteryElement battery)
	{
		bool isLoopComplete = false;
		if (!IsElementPassSignal())
		{
			return false;
		}
		foreach (var output in _outputs)
		{
			isLoopComplete = isLoopComplete|| output.UpdateRecursive(stillComplete, index + (_resistance ? 1 : 0), battery);
		}
		if (isLoopComplete)
			OnElementActive();
		//Debug.Log(this.GetType().ToString()+isLoopComplete);
		return isLoopComplete;
	}
}
