using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
/// <summary>
/// ���� ���� ����������� ����� ������ ����� � ��������, ������ ��� ��������� ���������� ��� �'������� �������� � ��������
/// </summary>
public class NodeInput : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	//��������� - ������ ��� ����� ���������� ��� ����� �'������� ��������, ��� �� ���� �� �������
	//����� - ��'��� ���� ������ �'����� ��������
	//����� - ������ �����/������
	public delegate void OnDisableHanlder(NodeInput wireDot);
	public event OnDisableHanlder OnDotHide;

	//������� ������ ������� ��� ��������
	[SerializeField]
	private SpriteShape _spriteShape;
	//������ ����� 
	private HashSet<NodeInput> outputs = new();
	//�������
	private List<ConnectionWireRespresentation> wires = new();
	[SerializeField]
	//������ ������� ��� ��������� ���������
	private GameObject _wireObjectPrefab;
	//������ ������� ��� ��������
	private Spline _spline;
	private UnityEngine.Object _wireObject;
	
	//������� ��������� ��� ������
	public List<NodeInput> GetNodes()
	{
		return outputs.ToList();
	}
	//ĳ� ��� �������� �� ���� �'�������
	public void OnDrop(PointerEventData eventData)
	{
		//�������� �� �'������� �� �������� � ����� � ��������
		/*if (eventData.selectedObject == this.gameObject)
			return;*/

		//�������� ����� � ����� �������� ��������� �� ������ ��� ��'��� �� ���� � ���� ������
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
		//��������� �� ����� ������� �� ��������� �'�������
		foreach (var wire in wires)
		{
			Destroy(wire.gameObject);
		}
		//������� ������ �'������
		wires.Clear();
		//�������� ��'��� � ���� ��� ������� �������� ����� ����� ��� ���� ��� �� ��� �� ����������� ��� (layer) ����������
		Transform parent = FindObjectOfType<GridLayerInfo>().WireLayer.transform;
		//��� ������� �'������� ��������� �����
		foreach (NodeInput line in outputs)
		{
			//�������� ���������� ���� �������, ����� ��� �����
			Vector3 pos1 = line.GetComponent<RectTransform>().position;
			Vector3 pos2 = this.GetComponent<RectTransform>().position;
			
			//��������� ����� ����� �� �������� �� ���������� ����
			GameObject lineObject = Instantiate(_wireObjectPrefab);
			lineObject.transform.SetParent(transform);
			//������� - ������� ����� �� ����� �������
			lineObject.transform.position = (pos1 + pos2) / 2;
			lineObject.transform.localScale = Vector3.one;
			var rt = lineObject.GetComponent<RectTransform>();

			//������������:
			//1. ����� �������
			rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);
			//2. ���� �������
			rt.rotation = Quaternion.Euler(0, 0, CustomMathFunctions.GetAngleBetween(pos1,pos2));

			ConnectionWireRespresentation wire = lineObject.GetComponent<ConnectionWireRespresentation>();
			//����������� 䳿 ��� ������� �� �������
			//1. ��������� �'�������
			wire.onPressed += () => RemoveOutput(line);
			//2. ��������� �������
			wire.onPressed += () => line.UpdateLines();


			wires.Add(wire);

			rt.SetParent(parent);
			//rt.sizeDelta = new Vector2(Vector3.Distance(pos2, pos1)*100+25, 25);
		}
	}
	private void OnDisableHandler(NodeInput disablingDot)
	{
		//ĳ� ��� ��� �������� 
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
		//��������� ���������� ����� �� ��� ���������
		_wireObject = new GameObject();
		var controller = _wireObject.AddComponent<SpriteShapeController>();
		controller.spriteShape = _spriteShape;
		controller.splineDetail = 4;
		_spline = controller.spline;
		_spline.isOpenEnded = true;
	}

	public void OnDrag(PointerEventData eventData)
	{
		//��������� ���� ����������� ������� ����� ���� ��� ��������
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
		//��������� ���������� �����
		Destroy(_wireObject);
	}
}
