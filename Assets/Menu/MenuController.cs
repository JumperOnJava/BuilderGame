using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	static public MenuController Instance;// { get { return GameObject.FindGameObjectWithTag("GameController").GetComponent<MenuController>(); }}
	[SerializeField]
	private List<BuilderData> _levels = new List<BuilderData>();
	[SerializeField]
	private GameObject _playButton;
	[SerializeField]
	private GameObject _levelMenu;
	[SerializeField]
	private GridLayoutGroup _levelGrid;
	[SerializeField]
	private OpenLevelButton _gridButtonTemplate;
	private int _currentLevelId;
	private string _menuScene = "MainMenu";
	private void Awake()
	{
		if(!File.Exists(SaveSystem.GetSavePath()))
		{
			File.WriteAllText(SaveSystem.GetSavePath(), "ilevel,0");
		}
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);
		_playButton.SetActive(true);
		_levelMenu.SetActive(false);
		DontDestroyOnLoad(this);
	}
	public void LoadSpecificLevel(int levelId)
	{
		_currentLevelId = levelId;
		_levelMenu.SetActive(false);
		try
		{
			SceneManager.LoadSceneAsync(_levels[levelId].Scene).completed += (AsyncOperation t) =>
			{
				BuilderUiController controller = FindObjectOfType<BuilderUiController>();

				controller.Init(_levels[levelId]);
				
				try
				{
				}
				catch(Exception e)
				{
					if (levelId> _levels.Count)
					{
						Debug.Log("Tried to load next level, but current level was last level");
					}
					else
					{
						Debug.LogError("Coudn't load next level, not finish");
						Debug.LogError(e);

					}
				}
			};
		}
		catch
		{
			LoadMenuScene();
		}
	}
	public void OnFinish()
	{
		SaveData.Level = Mathf.Max(_currentLevelId + 1, SaveData.Level);
	}
	public void LoadNextLevel()
	{
		LoadSpecificLevel(_currentLevelId + 1);
	}

	public void LoadMenuScene()
	{
		SceneManager.LoadSceneAsync(_menuScene).completed += (AsyncOperation t) => OpenLevelMenu();
	}
	public void OpenLevelMenu()
	{
		Debug.Log(SaveData.Level);
		_playButton.SetActive(false);
		_levelMenu.SetActive(true);
		UpdateLevelMenu();
	}
	public void ExitFromGame()
	{
		Application.Quit();
	}
	public void UpdateLevelMenu()
	{
		Debug.Log(SaveData.Level);
		foreach(Transform child in _levelGrid.transform)
		{
			Destroy(child.gameObject);
		}
		var i = 0;
		foreach(BuilderData level in _levels)
		{
			OpenLevelButton button = Instantiate(_gridButtonTemplate);
			button.Init(i,i> SaveData.Level);
			button.transform.SetParent(_levelGrid.transform);
			i++;
		}
	}
	public void ResetLevel()
	{
		SaveData.Level = 0;
	}
	private void Update()
	{
		//UpdateLevelMenu();	
	}
}
