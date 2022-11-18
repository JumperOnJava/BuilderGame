using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;
//Клас який зберігає логіку будь якого електричного елементу
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
	public abstract void SetElementActive(bool active);

	protected bool _resistance = true;
	//Рекурсивне оновлення
	public virtual bool UpdateRecursive(bool stillComplete, int resistance, BatteryElement battery)
	{
		bool isLoopComplete = false;
		//Перевіряємо чи сенсор повинен передавати сигнал
		if (!IsElementPassSignal())
		{
			return false;
		}
		//питаємо у кожного наступного елемента чи повне коло

		foreach (var output in _outputs)
		{
			isLoopComplete = isLoopComplete || output.UpdateRecursive(stillComplete, resistance + (_resistance ? 1 : 0), battery);
		}
		// якщо хоч один з них відповідає що повне, то активуємо цей елемент
		SetElementActive(isLoopComplete);
		CameraBoundsInEditor.DebugPoint(transform.position, 0.3f);

		//поветраємо чи повне це коло попередньому елементу
		return isLoopComplete;
	}
}
