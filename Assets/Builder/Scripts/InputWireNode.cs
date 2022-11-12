using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
public class InputWireNode : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	
	public delegate void OnDisableHanlder(InputWireNode wireDot);
	public event OnDisableHanlder OnDotHide;
	public OutputWireNode OutputNode;
	public SpriteShape SpriteShape;
	private HashSet<InputWireNode> outputs = new();
	private List<ConnectionWireRespresentation> wires = new();
	[SerializeField]
	private GameObject _wireObjectPrefab;
	[SerializeField]
	private Transform _wireParentun;

	private Spline _spline;
	private UnityEngine.Object _wireObject;

	public List<InputWireNode> GetWireDots()
	{
		List<InputWireNode> _outputList = new();
		foreach (var output in outputs)
		{
			_outputList.Add(output);
		}
		return _outputList;
	}
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.selectedObject == OutputNode.gameObject)
			return;
		//Debug.Log($"Recieved Drop Event from {eventData.selectedObject}");
		var node = eventData.selectedObject.GetComponent<OutputWireNode>();
		/*if (node.InputNode.outputs.Contains(this) || outputs.Contains(node.InputNode))
		{
			RemoveOutput(node.InputNode);
			node.InputNode.RemoveOutput(this);
		}
		else*/
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
			rt.rotation = Quaternion.Euler(0, 0, CustomVectorFunctions.GetAngleBetween(pos1,pos2));
			rt.position = new Vector3(rt.position.x, rt.position.y,100);
			ConnectionWireRespresentation wire = lineObject.GetComponent<ConnectionWireRespresentation>();
			wire.onPressed += () => RemoveOutput(line);
			wire.onPressed += () => line.UpdateLines();
			Debug.Log("finished creating line");
			wires.Add(wire);
			rt.SetParent(parent);
			rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);
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
