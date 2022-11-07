using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class OpenLevelButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _textObject;
	[SerializeField]
	private Button _thisButton;
	[SerializeField]
	private GameObject _lockIcon;
	private int _levelId;
	public void Init(int number,bool isLocked)
	{
		_lockIcon.SetActive(isLocked);
		_levelId = number;
		_textObject.text = $"{_levelId+1}";
	}
	public void OnClick()
	{
		MenuController.Instance.LoadSpecificLevel(_levelId);
	}
}
