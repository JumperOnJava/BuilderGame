using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
/// <summary>
/// Клас який представляє собою іконку мінуса в редакторі, зберігає всю необхідну інформацію про з'єднання елементів в редакторі
/// </summary>
public class InputWireNode : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	//Наведення - процес при якому користувач вже почав з'єднання елементів, але ще його не закінчив
	//Провід - об'єкт який показує з'єднані елементи
	//Вузол - іконка плюса(вихід) та мінуса(входу)
	public delegate void OnDisableHanlder(InputWireNode wireDot);
	public event OnDisableHanlder OnDotHide;
	
	//Вузол мінуса цього елемента
	public OutputWireNode OutputNode;
	//Зовнішній вигляд провода при наведенні
	public SpriteShape SpriteShape;
	//Вихідні вузли 
	private HashSet<InputWireNode> outputs = new();
	//Проводи
	private List<ConnectionWireRespresentation> wires = new();
	[SerializeField]
	//Шаблон провода при завершенні наведення
	private GameObject _wireObjectPrefab;
	//Шаблон провода при наведенні
	private Spline _spline;
	private UnityEngine.Object _wireObject;
	
	//Функція отримання всіх виходів
	public List<InputWireNode> GetNodes()
	{
		return outputs.ToList();
	}
	//Дії при наведенні на себе з'єднання
	public void OnDrop(PointerEventData eventData)
	{
		//перевірка чи з'єднання не почалося з цього ж елементу
		if (eventData.selectedObject == OutputNode.gameObject)
			return;

		//отримуємо вивід (плюс) з якого почалося наведення та додаємо цей об'єкт як один з його виводів
		if(eventData.selectedObject.TryGetComponent<OutputWireNode>(out var node))
		node.InputNode.AddOutput(this);
	}
	public void RemoveOutput(InputWireNode wireDot)
	{
		outputs.Remove(wireDot);
		if (outputs.Count == 0)
		UpdateLines();
	}
	public void AddOutput(InputWireNode wireDot)
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
		foreach (InputWireNode line in outputs)
		{
			//отримуємо координати кінці провода, тобто два вузли
			Vector3 pos1 = line.GetComponent<RectTransform>().position;
			Vector3 pos2 = OutputNode.GetComponent<RectTransform>().position;
			
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
	private void OnDisableHandler(InputWireNode disablingDot)
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

		_spline.InsertPointAt(0, GetComponent<RectTransform>().position);
		_spline.InsertPointAt(1, Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
		_spline.RemovePointAt(2);
		_spline.RemovePointAt(2);

		//Debug.Log(_spline.GetPointCount());
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Destroy(_wireObject);
		var selobj = eventData.selectedObject;
		var comp = GetComponent<InputWireNode>();
		var inputNode = comp;
		inputNode.AddOutput(this);
	}
}
