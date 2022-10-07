using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ElementContainer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
	public GridCell Cell;
	private GameObject _dragSprite;
	private Camera _camera;
	public void Init(GridCell cell)
	{
		this.Cell = cell;
	}
	public bool ShouldDrag()
	{

		if (Cell.Type == ElementType.Empty) return false;
		if (!AllowSend()) return false;
		return true;
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!ShouldDrag())
			return;
		_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		_dragSprite = new GameObject();
		var sprite = _dragSprite.AddComponent<SpriteRenderer>();
		sprite.sprite = Cell.GetInfo().Sprite;
		sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (int)Cell.Rotation*90));
		sprite.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		_dragSprite.transform.SetParent(_camera.gameObject.transform);
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (!ShouldDrag())
			return;
		_dragSprite.transform.position = _camera.ScreenToWorldPoint(Input.mousePosition).ComponentMultiply(new Vector3(1, 1, 0));/// _canvas.transform.lossyScale.x ;;
		Debug.DrawLine(Vector3.zero, _dragSprite.transform.position, Color.red, 0.2f);
	}
	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.selectedObject.TryGetComponent<ElementContainer>(out var _))
			return;
		var container = eventData.selectedObject.GetComponent<ElementContainer>();
		if (container == null)
			return;
		//if (container.Cell.Type == Cell.Type)
		//return;
		if (!(container.AllowSend() && AllowRecieve()))
			return;
		var c1 = container.Cell;
		var c2 = Cell;
		container.OnSuccessfullSend(c2);
		OnSuccessfullRecieve(c1);
	}
	public abstract bool AllowRecieve();
	public abstract bool AllowSend();
	public abstract void OnSuccessfullRecieve(GridCell recieveCell);
	public abstract void OnSuccessfullSend(GridCell SendCell);
	public void OnEndDrag(PointerEventData eventData)
	{
		Destroy(_dragSprite);
	}
}