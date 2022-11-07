using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
	private static TutorialStep emptyStep = new TutorialStep();
	public TutorialStep NextStep = emptyStep;
	public TutorialStep PreviousStep = emptyStep;
	[SerializeField]
	private bool _isFirst = false;
	void Awake()
    {
		if (NextStep == null)
			NextStep = emptyStep;
		if (PreviousStep == null)
			PreviousStep = emptyStep;

		gameObject.SetActive(_isFirst);
		NextStep.PreviousStep = this;
		int empty = 0;
		if (NextStep == emptyStep)
			empty++;
		if (PreviousStep == emptyStep)
			empty++;
		switch (empty)
		{
			case 1:
				Debug.LogWarning($"Warning {this} have empty steps, if there is less than 3 of this warning everything");
				break;
			case 2:
				Debug.LogError($"{this} does not have previous and next steps, something went wrong");
				break;
		}

	}
	public void GoNextStep()
	{
		NextStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
		NextStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
	public void GoPreviousStep()
	{
		PreviousStep.gameObject.SetActive(true);
		gameObject.SetActive(false); 
		PreviousStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
