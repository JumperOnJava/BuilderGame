using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class OutputWireNode : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField]
	public SpriteShape SpriteShape;
	private GameObject _wireObject;
	private Spline _spline;
	public InputWireNode InputDot;

	public delegate void OnOutputAdded();
	public event OnOutputAdded OnOutputCreated;

	public long RandID;
	public void Awake()
	{
		RandID = (long)UnityEngine.Random.Range(0, int.MaxValue);
	}
	public void OutputAdded()
	{
		//OnOutputCreated();
	}
	public void Update()
	{
		//InputDot.GetWireDots();
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		Debug.Log("WireDot Started drag");
		_wireObject = new GameObject();
		var controller = _wireObject.AddComponent<SpriteShapeController>();
		controller.spriteShape = SpriteShape;
		controller.splineDetail = 4;
		_spline = controller.spline;
		_spline.isOpenEnded = true;
		_spline.InsertPointAt(0, GetComponent<RectTransform>().position);
		_spline.InsertPointAt(1, GetComponent<RectTransform>().position);
	}

	public void OnDrag(PointerEventData eventData)
	{
		_spline.RemovePointAt(1);
		_spline.InsertPointAt(1, Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Destroy(_wireObject);
	}
	
}
