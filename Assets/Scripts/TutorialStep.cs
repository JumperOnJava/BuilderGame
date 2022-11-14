using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Клас який допомагає переключати кроки допомоги в початку більшості рівнів
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
		//якщо наступний/минулий крок не встановлено то встановлюємо заглушку
		if (NextStep == null)
			NextStep = emptyStep;
		if (PreviousStep == null)
			PreviousStep = emptyStep;
		//якщо цей крок перший то активуємо, інакше ховаємо його
		gameObject.SetActive(_isFirst);
		//встановлюємо минулим кроком наступного кроку цей крон
		NextStep.PreviousStep = this;

	}
	//перехід на наступний крок
	public void GoNextStep()
	{
		NextStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
		NextStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
	//перехід на минулий крок
	public void GoPreviousStep()
	{
		PreviousStep.gameObject.SetActive(true);
		gameObject.SetActive(false); 
		PreviousStep.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
