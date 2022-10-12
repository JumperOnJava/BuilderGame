using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
public class InputWireNode : MonoBehaviour, IDropHandler
{
	
	public delegate void OnDisableHanlder(InputWireNode wireDot);
	public event OnDisableHanlder OnDotHide;
	public delegate void OnRecieveOutput();
	public event OnRecieveOutput OnRecievedFirstOutput;
	public delegate void OnDeleteOutput();
	public event OnDeleteOutput OnDeletedLastOutput;
	
	public SpriteShape SpriteShape;
	private HashSet<InputWireNode> outputs = new();
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
		if (eventData.selectedObject == gameObject)
			return;
		Debug.Log($"Recieved Drop Event from {eventData.selectedObject}");
		var node = eventData.selectedObject.GetComponent<OutputWireNode>();
		if (node.InputDot.outputs.Contains(this) || outputs.Contains(node.InputDot))
		{
			RemoveOutput(node.InputDot);
			node.InputDot.RemoveOutput(this);
		}
		else
		{
			node.InputDot.AddOutput(this);
		}
		node.OutputAdded();
	}
	public void RemoveOutput(InputWireNode wireDot)
	{
		outputs.Remove(wireDot);
		if (outputs.Count == 0)
		UpdateLines();
	}
	private void AddOutput(InputWireNode wireDot)
	{
		if (outputs.Contains(wireDot))
			outputs.Remove(wireDot);
		else
			outputs.Add(wireDot);
		if (outputs.Count == 1)
		{

		}
		UpdateLines();
		wireDot.OnDotHide += OnDisableHandler;
	}
	private void UpdateLines()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		foreach (InputWireNode line in outputs)
		{
			Vector3 pos1 = line.GetComponent<RectTransform>().position;
			Vector3 pos2 = GetComponent<RectTransform>().position;
			GameObject lineObject = new();
			lineObject.transform.SetParent(transform);
			var controller = lineObject.AddComponent<SpriteShapeController>();
			var spline = controller.spline;
			controller.spriteShape = SpriteShape;
			controller.splineDetail = 4;
			spline.isOpenEnded = true;
			spline.InsertPointAt(0, pos1);
			spline.InsertPointAt(1, pos2);
		}
	}
	private void OnDisableHandler(InputWireNode disablingDot)
	{
		Debug.Log($"wireDot {disablingDot}");
		RemoveOutput(disablingDot);
	}
	public void DisableNode()
	{
		OnDotHide += (_) => { };
		foreach(var output in outputs)
		{

		}
		OnDotHide.Invoke(this);
		outputs.Clear();
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		gameObject.SetActive(false);
	}

	public void EnableNode()
	{
		gameObject.SetActive(true);
	}

}
