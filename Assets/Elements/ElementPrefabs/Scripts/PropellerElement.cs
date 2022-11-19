using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PropellerElement : EngineElement
{
	private Rigidbody2D _thisBody;
	[SerializeField]
	private float _power=10;
	[SerializeField]
	private float _maxVelocity=10;
	[SerializeField]
	private Animator _animator;
	public void Awake()
	{
		_thisBody = GetComponent<Rigidbody2D>();
	}
	public override void OnActiveThisFrame()
	{
		//_thisBody.velocity = transform.up * _power;
		_thisBody.AddForce(transform.up*_power * Time.fixedDeltaTime, ForceMode2D.Impulse);
		//Debug.Log(_thisBody.velocity.magnitude);
		if (_thisBody.velocity.magnitude > _maxVelocity)
		{
			_thisBody.AddForce(-_thisBody.velocity.normalized * (_thisBody.velocity.magnitude - _maxVelocity) * 2 * Time.fixedDeltaTime, ForceMode2D.Force);
		}
		_animator.SetBool("Rotating", true);
	}

	public override void OnInactiveThisFrame()
	{
		_animator.SetBool("Rotating", false);
	}
}
