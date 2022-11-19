using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//���� ���� ������ ����� �����
public class LampElement : EngineElement
{
	//��'��� ����� 
	[SerializeField]
	private GameObject _lightObject;
	private ArtificialLight _thisLight;

	public override void OnActiveThisFrame()
	{
	}

	public override void OnInactiveThisFrame()
	{
	}
	public void Awake()
	{
		_thisLight = GetComponent<ArtificialLight>();
	}
	//�������� ����� � ��������� �� ���� �� ������� �� ��� ������� ������
	public override void SetElementActive(bool b)
	{
		_lightObject.SetActive(b);
	}
	public void Update()
	{
	}
}
