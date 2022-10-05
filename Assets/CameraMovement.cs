using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _mousePos => _camera.ScreenToWorldPoint(Input.mousePosition);
    private Vector3 _prevMousePos = Vector3.one;
    private Vector3 _mouseDelta =>  _prevMousePos - _mousePos;
    private Vector3 _targetPos;

    private float _targetSize;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _targetPos = transform.position;
        _targetSize = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            _targetPos += _mouseDelta;
        }
        int n = 8;
        transform.position = (transform.position * (n - 1) + (_targetPos)) / n;
        transform.position = new Vector3(transform.position.x,transform.position.y,-1);

        _targetSize += -Input.mouseScrollDelta.y;
        _targetSize = Mathf.Clamp(_targetSize, 1, 16);
        int m = 8;
        _camera.orthographicSize = (_camera.orthographicSize * (m-1) + _targetSize)/m;
    }
    private void LateUpdate()
    {
        _prevMousePos = _mousePos;
    }
}
