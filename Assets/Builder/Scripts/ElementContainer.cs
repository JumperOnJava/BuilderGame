using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
//Клас який має частину логіки пов'язану з перетягуванням елементів у редакторі
public abstract class ElementContainer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler 
{
	private static GridCell CursorSlot;
	public GridCell Cell;
	private GameObject _dragSprite;
	private Camera _camera;
	public void Init(GridCell cell)
	{
		this.Cell = cell;
	}
	//перевірка чи можна перетягнути елемент з цього контейнера
	public bool ShouldDrag()
	{
		//не можна якщо елемент пустий
		if (Cell.Type == ElementType.Empty) return false;
		//не можна якщо сам контейнер забороняє
		if (!AllowSend()) return false;
		return true;
	}
	//При початку перетягування
	public void OnBeginDrag(PointerEventData eventData)
	{ 
		//перевіряємо чи можна перетягувати
		if (!ShouldDrag())
			return;
		//знаходимо камеру
		_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		//Створюємо спрайт при перетягуванні та налаштовуємо 
		_dragSprite = new GameObject();
		SpriteRenderer sprite = _dragSprite.AddComponent<SpriteRenderer>();
		sprite.sprite = Cell.GetInfo().Sprite;
		sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (int)Cell.Rotation*90));
		sprite.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		_dragSprite.transform.SetParent(_camera.gameObject.transform);
		//CursorSlot = this.Cell;
		//this.Cell = GridCell.EmptyCell;
	}
	//кожен кадр при перетягуванні
	public void OnDrag(PointerEventData eventData)
	{
		//перевіряємо чи можна перетягувати
		if (!ShouldDrag())
			return;
		//переміщаємо елемент за курсором
		_dragSprite.transform.position = _camera.ScreenToWorldPoint(Input.mousePosition).ComponentMultiply(new Vector3(1, 1, 0));/// _canvas.transform.lossyScale.x ;;
	}
	//Дії при отриманні елемента
	public void OnDrop(PointerEventData eventData)
	{
		try
		{
			//перевіряємо чи початковий елемент є контейнером
			if (!eventData.selectedObject.TryGetComponent<ElementContainer>(out var _))
				return;
			var container = eventData.selectedObject.GetComponent<ElementContainer>();
			if (container == null)
				return;
			//Якщо початковий елемент дозволяє відправляти елементи та цей елемент дозволяє отримувати
			if (!(container.AllowSend() && AllowRecieve()))
				return;
			if (GetType() != typeof(ElementListButton))
			if (Cell.Type != ElementType.Empty)
				return;
			//викликаємо фунцкію обробки отримання елементу (кожен елемент може змініти логіку при отриманні)
			DropHandler(container);
		}
		catch { };

	}
	public virtual void DropHandler(ElementContainer container)
	{
		//якщо функція не перевизначена то на кожну сторону перенесення передаємо інформацію про іншу сторону 
		GridCell c1 = container.Cell;
		GridCell c2 = Cell;
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
