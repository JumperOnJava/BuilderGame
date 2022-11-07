using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToLevelMenuButton : MonoBehaviour
{
    public void OnClick()
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<MenuController>().LoadMenuScene();
	}
}
