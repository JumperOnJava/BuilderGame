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

	public delegate void OnDisableHanlder(InputWireNode wireDot);
	public event OnDisableHanlder OnDotHide;
	
	//іконка мінуса
	public OutputWireNode OutputNode;
	//Зовнішній вигляд провода при наведенні
	public SpriteShape SpriteShape;
	//Виходи елементів
	private HashSet<InputWireNode> outputs = new();
	//
	private List<ConnectionWireRespresentation> wires = new();
	[SerializeField]
	//Шаблон провода
	private GameObject _wireObjectPrefab;

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
		var node = eventData.selectedObject.GetComponent<OutputWireNode>();
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
		/*if (outputs.Contains(wireDot))
			outputs.Remove(wireDot);
		else*/
		if (wireDot != this)
			outputs.Add(wireDot);
		/*if (outputs.Count == 1)
		{

		}*/
		UpdateLines();
		wireDot.OnDotHide += OnDisableHandler;
	}
	public void UpdateLines()
	{
		foreach (var wire in wires)
		{
			Destroy(wire.gameObject);
		}
		wires.Clear();
		Transform parent = FindObjectOfType<GridLayerInfo>().WireLayer.transform;
		foreach (InputWireNode line in outputs)
		{
			Vector3 pos1 = line.GetComponent<RectTransform>().position;
			Vector3 pos2 = OutputNode.GetComponent<RectTransform>().position;
			GameObject lineObject = Instantiate(_wireObjectPrefab);
			lineObject.transform.SetParent(transform);
			lineObject.transform.position = (pos1 + pos2) / 2;
			lineObject.transform.localScale = Vector3.one;
			var rt = lineObject.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);
			Debug.Log($"{pos1};;;{pos2}");
			rt.rotation = Quaternion.Euler(0, 0, CustomMathFunctions.GetAngleBetween(pos1,pos2));
			rt.position = new Vector3(rt.position.x, rt.position.y,100);
			ConnectionWireRespresentation wire = lineObject.GetComponent<ConnectionWireRespresentation>();
			wire.onPressed += () => RemoveOutput(line);
			wire.onPressed += () => line.UpdateLines();
			Debug.Log("finished creating line");
			wires.Add(wire);
			rt.SetParent(parent);
			rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);

			/*line.UpdateLines();
			UpdateLines();*/
		}
	}
	private void OnDisableHandler(InputWireNode disablingDot)
	{
		Debug.Log($"wireDot {disablingDot}");
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
