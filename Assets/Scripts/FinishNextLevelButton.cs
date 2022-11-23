using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Кнопка переходу на наступний рівень
public class FinishNextLevelButton : MonoBehaviour
{
    public void OnClick()
	{
		//завантажуємо наступний рівень
		MenuController.Instance.LoadNextLevel();
	}
}
