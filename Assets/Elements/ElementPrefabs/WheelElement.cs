using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelElement : EngineElement
{
    [SerializeField]
    private Rigidbody2D _jointWheelHolder;
    public override GameObject GetJointObject()
    {
        return _jointWheelHolder.gameObject;
    }
	public override void OnActiveThisFrame()
	{
		var hinge = GetComponentInChildren<HingeJoint2D>();
		hinge.useMotor = true;
		Debug.Log($"{GetType()} is activated");
	}
	public override void OnInactiveThisFrame()
	{
		var hinge = GetComponentInChildren<HingeJoint2D>();
		hinge.useMotor = false;
		Debug.Log($"{GetType()} is deactivated");
	}
}
