using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ���� ���� �������� ����������� ����� �������� � ������� ������� ����
/// 
/// </summary>
public class TutorialStep : MonoBehaviour
{
	private static TutorialStep emptyStep = new TutorialStep();
	public TutorialStep NextStep = emptyStep;
	public TutorialStep PreviousStep = emptyStep;
	[SerializeField]
	private bool _isFirst = false;
	void Awake()
    {
		//���� ���������/������� ���� �� ����������� �� ������������ ��������
		if (NextStep == null)
			NextStep = emptyStep;
		if (PreviousStep == null)
			PreviousStep = emptyStep;
		//���� ��� ���� ������ �� ��������, ������ ������ ����
		gameObject.SetActive(_isFirst);
		//������������ ������� ������ ���������� ����� ��� ����
		NextStep.PreviousStep = this;

	}
	//������� �� ��������� ����
	public void GoNextStep()
	{
		NextStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
		NextStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
	//������� �� ������� ����
	public void GoPreviousStep()
	{
		PreviousStep.gameObject.SetActive(true);
		gameObject.SetActive(false); 
		PreviousStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
