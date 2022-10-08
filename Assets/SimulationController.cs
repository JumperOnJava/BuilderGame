using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera => Camera.main;
	[SerializeField]
	private BuilderUiController _builderController;
	[SerializeField]
	public GameObject BuildParent;
	private void OnEnable()
	{
		_builderController.gameObject.SetActive(false);
	}
	public void ReturnToBuilder()
	{
		Debug.Log("click");
		for (int i=0;i<BuildParent.transform.childCount;i++)
		{
			Destroy(BuildParent.transform.GetChild(i).gameObject);
		}
		_builderController.gameObject.SetActive(true);
		gameObject.SetActive(false);
	}
}
