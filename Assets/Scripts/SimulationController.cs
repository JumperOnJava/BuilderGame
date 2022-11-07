using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationController : MonoBehaviour
{
	[SerializeField]
	private BuilderUiController _builderController;
	public GameObject BuildParent;
	private void OnEnable()
	{
		_builderController.gameObject.SetActive(false);
	}

	public void ReturnToBuilder()
	{
		for (int i=0;i<BuildParent.transform.childCount;i++)
		{
			Destroy(BuildParent.transform.GetChild(i).gameObject);
		}
		_builderController.gameObject.SetActive(true);
		gameObject.SetActive(false);

		Camera.main.GetComponent<CameraMovement>().enabled = true;
		Camera.main.GetComponent<CameraMovement>().MoveTargetPosTo(_builderController.gameObject);
	}
	private void OnDisable()
	{
		ReturnToBuilder();
	}
}
