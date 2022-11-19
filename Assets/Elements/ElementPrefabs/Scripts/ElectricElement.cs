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
	//Рекурсивне оновлення
	public virtual void UpdateRecursive()
	{
		//активуємо цей елемент якщо це двигун
		SetElementActive(true);
		//Перевіряємо чи елемент повинен передавати сигнал далі
		if (!IsElementPassSignal())
		{
			return;
		}
		//Подаємо сигнал на кожен наступний елемент
		foreach (var output in _outputs)
		{
			output.UpdateRecursive();
		}
		return;
	}
}
