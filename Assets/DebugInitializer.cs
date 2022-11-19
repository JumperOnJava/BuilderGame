using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebugInitializer : MonoBehaviour
{
	[SerializeField]
	private BuilderData _data;
	[SerializeField]
	private int fps=60;
	void Start()
    {
		var menucontroller = MenuController.Instance;
		if(menucontroller == null)
		{
			FindObjectOfType(typeof(BuilderUiController)).GetComponent<BuilderUiController>().Init(_data);
		}
    }
	private void Update()
	{
		Application.targetFrameRate = fps;
		//Debug.Log(1.0f/ Time.fixedDeltaTime);
	}

}
