using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishNextLevelButton : MonoBehaviour
{
    public void OnClick()
	{
		MenuController.Instance.LoadNextLevel();
	}

}
