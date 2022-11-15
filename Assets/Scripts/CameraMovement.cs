using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.U2D;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;
	[SerializeField]
	private float _minSize = 2, _maxSize = 16;
	[SerializeField]
	public float _upClamp;
	[SerializeField]
	public float _downClamp;

	[SerializeField]
	public float _leftClamp;
	[SerializeField]
	public float _rightClamp;

    private Vector3 _mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private Vector3 _prevMousePos = Vector3.one;
    private Vector3 _mouseDelta =>  _prevMousePos - _mousePos;

	[SerializeField]
	public List<Vector2> _startPath;
	[SerializeField]
	private float _pathSpeed;

	[SerializeField]
	public Vector3 mousepos;
	[SerializeField]
	public Vector3 mousedelta;
	public float completedLenght = 0;

	[SerializeField]
    public Vector3 _targetPos;
    private float _targetSize;
	[SerializeField]
	private float _coreCounter=10;
	private float _coreTime=4;

	private List<GenericElement> _elements;
	
	IEnumerator PathCorountine()
	{
		var startSize = _targetSize;
		float length = 0;
		for (int i = 0; i < _startPath.Count - 1; i++)
		{
			length += Vector2.Distance(_startPath[i], _startPath[i + 1]);
		}
		for (; completedLenght <= length; completedLenght += _pathSpeed)
		{
			try
			//while (true)
			{
				completedLenght += Time.deltaTime * _pathSpeed;
				if (_coreCounter < _coreTime)
					break;
				float countLenght = 0;
				int i = 0;
				while (countLenght < completedLenght)
				{
					countLenght += Vector2.Distance(_startPath[i], _startPath[i + 1]);
					i++;
				}

				i = Mathf.Clamp(i, 1, _startPath.Count - 1);
				_targetSize = startSize + CustomMathFunctions.Lerp010(completedLenght / length)*4;
				completedLenght = Mathf.Clamp(completedLenght, 0, length);
				var last = countLenght - completedLenght;
				var dist = Vector2.Distance(_startPath[i], _startPath[i - 1]);
				//Debug.Log(last / dist);
				_targetPos = Vector2.Lerp(_startPath[i], _startPath[i - 1], last / dist);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				break;
			}
			yield return null;
		}
	}
	
	void Start()
    {
        _camera = GetComponent<Camera>();
		_targetPos = _camera.transform.position;
		_targetSize = _camera.orthographicSize;
		StartCoroutine(PathCorountine());
    }
    void FixedUpdate()
    {
		

		mousedelta = _mouseDelta;
		mousepos =_mousePos;
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
				_targetPos = GetCenterPoint();
			}
			catch 
			{
			}
		}

		_targetPos.x = Math.Clamp(_targetPos.x, _leftClamp, _rightClamp);
		_targetPos.y = Math.Clamp(_targetPos.y, _downClamp, _upClamp);

		transform.position = (transform.position * (n - 1) + (_targetPos)) / n;
        transform.position = new Vector3(transform.position.x,transform.position.y,-1);

        _targetSize += -Input.mouseScrollDelta.y*2;
        _targetSize = Mathf.Clamp(_targetSize, _minSize, _maxSize);
        _camera.orthographicSize = (_camera.orthographicSize * (m-1) + _targetSize)/m;

        _prevMousePos = _mousePos;
	}

	public void UpdateCenterElements(List<GenericElement> elements)
	{
		_elements = elements;
	}
	private Vector3 GetCenterPoint()
	{
		Vector3 center = Vector3.zero;
		foreach (GenericElement element in _elements)
		{
			center += element.transform.position;
		}
		center /= _elements.Count;
		return center;
	}

	internal void MoveTargetPosTo(GameObject gameObject)
	{
		_targetPos = gameObject.transform.position;
	}
}
