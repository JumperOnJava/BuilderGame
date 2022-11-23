using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//невеликий клас який конролює вимкнення/увімкнення редактора під час запуску рівню
public class SimulationController : MonoBehaviour
{
	[SerializeField]
	private BuilderUiController _builderController;
	public GameObject BuildParent;
	//при увімкненні вимикаємо редактор
	private void OnEnable()
	{
		_builderController.gameObject.SetActive(false);
	}
	//Дії для увімкнення редактора
	public void ReturnToBuilder()
	{
		//видаляємо все елементи візка
		for (int i=0;i<BuildParent.transform.childCount;i++)
		{
			Destroy(BuildParent.transform.GetChild(i).gameObject);
		}
		//вмикаємо редактор
		_builderController.gameObject.SetActive(true);
		//вимикаємо цей об'єкт
		gameObject.SetActive(false);
		//вмикаємо камеру та переміщаємо її до редактору
		Camera.main.GetComponent<CameraMovement>().enabled = true;
		Camera.main.GetComponent<CameraMovement>().MoveTargetPosTo(_builderController.gameObject);
	}
	//при вимкненні 
	private void OnDisable()
	{
		ReturnToBuilder();
	}
}
