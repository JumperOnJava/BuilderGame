using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

//Клас який представляє собою іконку плюса в редакторі, передає необхідну інформацію до вхідного вулза даного елемента 
public class OutputWireNode : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
	//зовнішній вигляд провода при наведенні
	[SerializeField]
	public SpriteShape SpriteShape;
	private GameObject _wireObject;
	private Spline _spline;

	//Вхідний вузол даного елемента
	public InputWireNode InputNode;
	//дії при початку наведення
	public void OnBeginDrag(PointerEventData eventData)
	{
		//створюємо тимчасовий провід на час наведення
		_wireObject = new GameObject();
		var controller = _wireObject.AddComponent<SpriteShapeController>();
		controller.spriteShape = SpriteShape;
		controller.splineDetail = 4;
		_spline = controller.spline;
		_spline.isOpenEnded = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		//оновлюємо кінці тимчасового провода кожен кадр при наведенні
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
		//видаляємо тимчасовий провід
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

	//Дії при наведенні на себе з'єднання
	public void OnDrop(PointerEventData eventData)
	{
		//Створюємо з'єднання якщо наведення почалося з мінусового вузла 
		if(eventData.selectedObject.TryGetComponent<InputWireNode>(out var recievedInput))
		InputNode.AddOutput(recievedInput);
	}
}
