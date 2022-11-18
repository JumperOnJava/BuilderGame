using System;
using System.Collections;
using System.Collections.Generic;	
using UnityEngine;

public abstract class EngineElement : ElectricElement
{
	[SerializeField]
	private bool _active = false;
	//public bool IsInverted = false;
	public void Start()
	{
		_resistance = true;
	}
	public override void SetElementActive(bool active)
	{
		this._active = active;
	}
	public override bool IsElementPassSignal(){return true;}
	public abstract void OnActiveThisFrame();
	public abstract void OnInactiveThisFrame();
	private void FixedUpdate()
	{
		if (_active)
			OnActiveThisFrame();
		else
			OnInactiveThisFrame();

		SetElementActive(false);
	}
}
