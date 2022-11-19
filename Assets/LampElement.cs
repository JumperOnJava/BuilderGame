using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Клас який реалізує логіку лампи
public class LampElement : EngineElement
{
	//Об'єкт світла 
	[SerializeField]
	private GameObject _lightObject;
	private ArtificialLight _thisLight;

	public override void OnActiveThisFrame()
	{
	}

	public override void OnInactiveThisFrame()
	{
	}
	public void Awake()
	{
		_thisLight = GetComponent<ArtificialLight>();
	}
	//Активуємо світло в залежності від того чи поданий на цей елемент сигнал
	public override void SetElementActive(bool b)
	{
		_lightObject.SetActive(b);
	}
	public void Update()
	{
	}
}
