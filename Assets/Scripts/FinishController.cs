using UnityEngine;

public class FinishController : MonoBehaviour
{
	[SerializeField]
	private GameObject _finishMenu;
	[SerializeField]
	private CameraMovement _camera;
	private void Start()
	{
		_finishMenu.SetActive(false);
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("triggered finish");
		if (other.gameObject.TryGetComponent<GenericElement>(out var elm))
		 _finishMenu.SetActive(true);
		MenuController.Instance.OnFinish();
		Camera.main.GetComponent<CameraMovement>().enabled = false;
	}
	public void HideMenu()
	{
		_finishMenu.SetActive(false);
		Camera.main.GetComponent<CameraMovement>().enabled = true;
	}
}
