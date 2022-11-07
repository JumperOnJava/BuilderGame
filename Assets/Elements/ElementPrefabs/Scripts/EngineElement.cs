using System.Collections;
using System.Collections.Generic;	
using UnityEngine;

public abstract class EngineElement : ElectricElement
{
	private bool activeThisFrame = false;
	//public bool IsInverted = false;
	public void Start()
	{
		_resistance = true;
	}
	public override void OnElementActive()
	{
		activeThisFrame = true;
	}
	public override bool IsElementPassSignal(){return true;}
	public abstract void OnActiveThisFrame();
	public abstract void OnInactiveThisFrame();
	private void LateUpdate()
	{
		if (activeThisFrame)
			OnActiveThisFrame();
		else
			OnInactiveThisFrame();
		activeThisFrame=false;
	}
}
