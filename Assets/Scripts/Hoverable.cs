using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Невеличкий клас, який контролює вспливаючі підказки на деяких кнопках
/// </summary>
public class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private GameObject _hoverObject;

	public void OnPointerEnter(PointerEventData eventData)
	{
		_hoverObject.SetActive(true);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		_hoverObject.SetActive(false);
	}
	private void OnDisable()
	{
		_hoverObject.SetActive(false);
	}
}
