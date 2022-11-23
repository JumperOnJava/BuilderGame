using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
//клас пропеллера
public class PropellerElement : EngineElement
{
	private Rigidbody2D _thisBody;
	[SerializeField]
	private float _power = 10;
	[SerializeField]
	private float _maxVelocity = 10;
	[SerializeField]
	private Animator _animator;
	public void Awake()
	{
		_thisBody = GetComponent<Rigidbody2D>();
	}
	//якщо пропеллер активний
	public override void OnActiveThisFrame()
	{
		//додаємо силу
		_thisBody.AddForce(transform.up * _power * Time.fixedDeltaTime, ForceMode2D.Impulse);
		//якщо швидкість дуже велика - зменшуємо її створюючи силу напрямлену в сторону протилежну стороні руху
		if (_thisBody.velocity.magnitude > _maxVelocity)
		{
			_thisBody.AddForce(-_thisBody.velocity.normalized * (_thisBody.velocity.magnitude - _maxVelocity) * 2 * Time.fixedDeltaTime, ForceMode2D.Force);
		}
		//Вмикаємо анімацію пропеллера
		_animator.SetBool("Rotating", true);
	}

	public override void OnInactiveThisFrame()
	{
		//Вмикаємо анімацію пропеллера
		_animator.SetBool("Rotating", false);
	}
}