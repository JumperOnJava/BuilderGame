using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;
	[SerializeField]
	private float _minSize = 2, _maxSize = 16;
	[SerializeField]
    private Vector3 _mousePos => _camera.ScreenToWorldPoint(Input.mousePosition);
    private Vector3 _prevMousePos = Vector3.one;
	[SerializeField]
    private Vector3 _mouseDelta =>  _prevMousePos - _mousePos;
	[SerializeField]
    private Vector3 _targetPos;
    private float _targetSize;
	[SerializeField]
	private float _coreCounter=0;
	private float _coreTime=4;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _targetPos = transform.position;
        _targetSize = _camera.orthographicSize;
    }
	
    void FixedUpdate()
    {
		float n = 8;
		float m = 3;
		if (Input.GetMouseButton(1))
        {
			n = 3;
			_coreCounter = 0;
			_targetPos += _mouseDelta;
        }
		else
		{
			_coreCounter += Time.deltaTime;
		}
		if(_coreCounter > _coreTime)
		{
			try
			{
				n = (1-Mathf.Clamp(_coreCounter-4,0,1))*30+1;
				var core = FindObjectOfType<CoreElement>().gameObject;
				_targetPos = core.transform.position;
			}
			catch 
			{

			}
		}
        transform.position = (transform.position * (n - 1) + (_targetPos)) / n;
        transform.position = new Vector3(transform.position.x,transform.position.y,-1);

        _targetSize += -Input.mouseScrollDelta.y*2;
        _targetSize = Mathf.Clamp(_targetSize, _minSize, _maxSize);
        _camera.orthographicSize = (_camera.orthographicSize * (m-1) + _targetSize)/m;

        _prevMousePos = _mousePos;
	}

	public void Activate()
	{

	}
}
