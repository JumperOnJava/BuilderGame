using System;
using System.Collections;
using System.Collections.Generic;	
using UnityEngine;
//Клас, який повинен наслідувати кожен елемент з двигуном/іншим споживачем енергії
public abstract class EngineElement : ElectricElement
{

	private bool _active = false;
	public void Start()
	{
		_resistance = true;
	}
	//Встановлення активності елементу
	public override void SetElementActive(bool active)
	{
		this._active = active;
	}
	public override bool IsElementPassSignal(){return true;}
	public abstract void OnActiveThisFrame();
	public abstract void OnInactiveThisFrame();
	//Кожен "тік" (50 раз в секунду)
	private void FixedUpdate()
	{
		//якщо елемент активний
		if (_active)
			//Виконуємо дії елементу, що відбуваються коли він активний 
			OnActiveThisFrame();
		else
			//інакше виконуємо дії елементу, що відбуваються коли він не активний 
			OnInactiveThisFrame();
		//вимикаємо елемент до отримання наступного сигналу
		SetElementActive(false);
	}
}
