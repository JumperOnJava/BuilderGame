using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerElement : EngineElement
{
	private Rigidbody2D _thisBody;
	[SerializeField]
	private float _power=10;
	[SerializeField]
	private float _maxVelocity=10;
	public void Awake()
	{
		_thisBody = GetComponent<Rigidbody2D>();
	}
	public override void OnActiveThisFrame()
	{
		_thisBody.AddForce(transform.up*_power, ForceMode2D.Force);
		Debug.Log(_thisBody.velocity.magnitude);
		if(_thisBody.velocity.magnitude > _maxVelocity)
		{ 
			_thisBody.AddForce(-_thisBody.velocity.normalized*(_thisBody.velocity.magnitude-_maxVelocity)*2, ForceMode2D.Force);
		}
	}

	public override void OnInactiveThisFrame()
	{
	}
}
