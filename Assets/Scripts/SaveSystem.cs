using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
	private static Dictionary<string,string> _saveData = new Dictionary<string, string>();
	public static void SetInteger(string key,int value)
	{
		RestoreDictionary();
		_saveData.TryAdd("i" + key, value.ToString());
		_saveData["i" + key]=value.ToString();
		SaveDictionary();
	}
	public static int GetInteger(string key)
	{
		RestoreDictionary();
		if(!_saveData.ContainsKey("i"+key))
			SetInteger(key,0);
		RestoreDictionary();
		return int.Parse(_saveData["i"+key]);
	}

	private static void SaveDictionary()
	{
		List<KeyValuePair<string,string>> saveList = _saveData.ToList();
		string saveString="";
		foreach(KeyValuePair<string,string> kvp in saveList)
		{
			saveString += $"{kvp.Key},{kvp.Value}\n";
		}
		File.WriteAllText(GetSavePath(), saveString);
	}
	private static void RestoreDictionary()
	{
		if (!File.Exists(GetSavePath()))
		{
			File.Create(GetSavePath());
			//File.WriteAllText(GetSavePath(), string.Empty);
		}
		var save = File.ReadAllLines(GetSavePath());
		_saveData.Clear();
		foreach(string line in save)
		{
			var kvp = line.Split(",");
			_saveData.Add(kvp[0], kvp[1]);
		}
	}

	public static string GetSavePath()
	{
		return Application.persistentDataPath + "/save.xdd";
	}

}
public static class SaveData
{
	public static int Level { get { return SaveSystem.GetInteger("level"); } set { SaveSystem.SetInteger("level", value); } }
}
