using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelElement : EngineElement
{
    [SerializeField]
    private Rigidbody2D _jointWheelHolder;
	[SerializeField]
	private Rigidbody2D _wheel;
	[SerializeField]
	private float _wheelSpeed;
	public override GameObject GetJointObject()
    {
        return _jointWheelHolder.gameObject;
    }
	public override void OnActiveThisFrame()
	{
		//якщо колесо активне збільшуємо швидкість повороту
		_wheel.AddTorque(-Time.fixedDeltaTime * _wheelSpeed);
	}
	public override void OnInactiveThisFrame()
	{
	}
}
