using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class OutputWireNode : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
	[SerializeField]
	public SpriteShape SpriteShape;
	private GameObject _wireObject;
	private Spline _spline;
	public InputWireNode InputNode;
	

	public long RandID;
	public void Awake()
	{
		RandID = (long)UnityEngine.Random.Range(0, int.MaxValue);
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log("WireDot Started drag");
		_wireObject = new GameObject();
		var controller = _wireObject.AddComponent<SpriteShapeController>();
		controller.spriteShape = SpriteShape;
		controller.splineDetail = 4;
		_spline = controller.spline;
		_spline.isOpenEnded = true;
	}

	public void OnDrag(PointerEventData eventData)
	{

		_spline.RemovePointAt(1);
		_spline.RemovePointAt(0);

		_spline.InsertPointAt(0, Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
		_spline.InsertPointAt(1, GetComponent<RectTransform>().position);
		_spline.RemovePointAt(2);
		_spline.RemovePointAt(2);

		//Debug.Log(_spline.GetPointCount());
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Destroy(_wireObject);

	}
	public void DisableNode()
	{
		gameObject.SetActive(false);
	}
	public void EnableNode()
	{
		gameObject.SetActive(true);
	}

	public void OnDrop(PointerEventData eventData)
	{
		if(eventData.selectedObject.TryGetComponent<InputWireNode>(out var recievedInput))
		InputNode.AddOutput(recievedInput);
	}
}
