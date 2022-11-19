using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
/// <summary>
/// Клас який представляє собою іконку вузла в редакторі, зберігає всю необхідну інформацію про з'єднання елементів в редакторі
/// </summary>
public class NodeInput : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	//Наведення - процес при якому користувач вже почав з'єднання елементів, але ще його не закінчив
	//Провід - об'єкт який показує з'єднані елементи
	//Вузол - іконка входу/виходу
	public delegate void OnDisableHanlder(NodeInput wireDot);
	public event OnDisableHanlder OnDotHide;

	//Зовнішній вигляд провода при наведенні
	[SerializeField]
	private SpriteShape _spriteShape;
	//Вихідні вузли 
	private HashSet<NodeInput> outputs = new();
	//Проводи
	private List<ConnectionWireRespresentation> wires = new();
	[SerializeField]
	//Шаблон провода при завершенні наведення
	private GameObject _wireObjectPrefab;
	//Шаблон провода при наведенні
	private Spline _spline;
	private UnityEngine.Object _wireObject;
	
	//Функція отримання всіх виходів
	public List<NodeInput> GetNodes()
	{
		return outputs.ToList();
	}
	//Дії при наведенні на себе з'єднання
	public void OnDrop(PointerEventData eventData)
	{
		//перевірка чи з'єднання не почалося з цього ж елементу
		/*if (eventData.selectedObject == this.gameObject)
			return;*/

		//отримуємо вузол з якого почалося наведення та додаємо цей об'єкт як один з його виводів
		if(eventData.selectedObject.TryGetComponent<NodeInput>(out var node))
		node.AddOutput(this);
	}
	public void RemoveOutput(NodeInput wireDot)
	{
		outputs.Remove(wireDot);
		if (outputs.Count == 0)
		UpdateLines();
	}
	public void AddOutput(NodeInput wireDot)
	{
		if (wireDot != this)
			outputs.Add(wireDot);
		UpdateLines();
		wireDot.OnDotHide += OnDisableHandler;
	}
	public void UpdateLines()
	{
		//Видаляємо всі минулі проводи які показують з'єднання
		foreach (var wire in wires)
		{
			Destroy(wire.gameObject);
		}
		//очищаємо список з'сднань
		wires.Clear();
		//отримуємо об'єкт в який нам потрібно помістити новий провід для того щоб він був на необхідному шарі (layer) інтерфейсу
		Transform parent = FindObjectOfType<GridLayerInfo>().WireLayer.transform;
		//для кожного з'єднання створюємо провід
		foreach (NodeInput line in outputs)
		{
			//отримуємо координати кінці провода, тобто два вузли
			Vector3 pos1 = line.GetComponent<RectTransform>().position;
			Vector3 pos2 = this.GetComponent<RectTransform>().position;
			
			//Створюємо новий провід за шаблоном та настроюємо його
			GameObject lineObject = Instantiate(_wireObjectPrefab);
			lineObject.transform.SetParent(transform);
			//позиція - середня точка між двома вузлами
			lineObject.transform.position = (pos1 + pos2) / 2;
			lineObject.transform.localScale = Vector3.one;
			var rt = lineObject.GetComponent<RectTransform>();

			//Встановлюємо:
			//1. Розмір провода
			rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);
			//2. Його поворот
			rt.rotation = Quaternion.Euler(0, 0, CustomMathFunctions.GetAngleBetween(pos1,pos2));

			ConnectionWireRespresentation wire = lineObject.GetComponent<ConnectionWireRespresentation>();
			//налаштовуємо дії при натиску по проводу
			//1. видаляємо з'єднання
			wire.onPressed += () => RemoveOutput(line);
			//2. оновлюємо проводи
			wire.onPressed += () => line.UpdateLines();


			wires.Add(wire);

			rt.SetParent(parent);
			//rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);
		}
	}
	private void OnDisableHandler(NodeInput disablingDot)
	{
		//Дії при при вимкненні 
		RemoveOutput(disablingDot);
		UpdateLines();
	}
	public void DisableNode()
	{
		OnDotHide += (_)=> { };
		OnDotHide.Invoke(this);
		outputs.Clear();
		foreach (var wire in wires)
		{
			Destroy(wire.gameObject);
		}
		wires.Clear();
		UpdateLines();
		gameObject.SetActive(false);
	}

	public void EnableNode()
	{
		gameObject.SetActive(true);
	}
	public void OnBeginDrag(PointerEventData eventData)
	{
		//створюємо тимчасовий провід на час наведення
		_wireObject = new GameObject();
		var controller = _wireObject.AddComponent<SpriteShapeController>();
		controller.spriteShape = _spriteShape;
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
}
