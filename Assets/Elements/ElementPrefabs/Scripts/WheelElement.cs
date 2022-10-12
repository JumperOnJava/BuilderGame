using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelElement : EngineElement
{
    [SerializeField]
    private Rigidbody2D _jointWheelHolder;
	[SerializeField]
	private Rigidbody2D _wheel;

	public override GameObject GetJointObject()
    {
        return _jointWheelHolder.gameObject;
    }
	public override void OnActiveThisFrame()
	{
		_wheel.AddTorque(-Time.deltaTime * 300);
		//var hinge = GetComponentInChildren<HingeJoint2D>();

	}
	public override void OnInactiveThisFrame()
	{
		//var hinge = GetComponentInChildren<HingeJoint2D>();
		//hinge.useMotor = false;
	}
}
