using UnityEngine;
//Клас який контролює все пов'язане з фінішом
public class FinishController : MonoBehaviour
{
	[SerializeField]
	private GameObject _finishMenu;
	[SerializeField]
	private CameraMovement _camera;
	private void Start()
	{
		//вимикаємо меню якщо воно увімкнено
		_finishMenu.SetActive(false);
	}
	//При заході у фініш
	private void OnTriggerEnter2D(Collider2D other)
	{
		//якщо об'єкт який торкнувся фініша є частиною візка
		if (other.gameObject.TryGetComponent<GenericElement>(out var elm))
		{
			//вмикаємо меню фінішу
			_finishMenu.SetActive(true);
			//розблоковуємо наступний рівень
			MenuController.Instance.OnFinish();
			//блокуємо камеру
			Camera.main.GetComponent<CameraMovement>().enabled = false;
		}
	}
	//фунція перезапуску рівня в меню фінішу
	public void HideMenu()
	{
		//вимикаємо меню фінішу
		_finishMenu.SetActive(false);
		//розблоковуємо камеру
		Camera.main.GetComponent<CameraMovement>().enabled = true;
	}
}
