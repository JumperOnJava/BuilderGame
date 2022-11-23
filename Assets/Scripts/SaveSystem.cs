using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//Клас для завантаження/збереження даних в файл
public static class SaveSystem
{
	//словник даних для збереження
	private static Dictionary<string,string> _saveData = new Dictionary<string, string>();
	//функція збереження цілого числа в файл
	public static void SetInteger(string key,int value)
	{
		//відновлюємо дані з файлу
		RestoreDictionary();
		//додаємо значення за клюмем в словник
		_saveData.TryAdd("i" + key, value.ToString());
		_saveData["i" + key]=value.ToString();
		//зберігаємо дані в файл
		SaveDictionary();
	}
	//функція отримання цілого числа з файлу
	public static int GetInteger(string key)
	{
		//відновлюємо дані з файлу
		RestoreDictionary();
		//якщо такого ключа не існує, створюємо зі значенням 0
		if(!_saveData.ContainsKey("i"+key))
			SetInteger(key,0);
		//зберігаємо дані в файл
		RestoreDictionary();
		//повертаємо число
		return int.Parse(_saveData["i"+key]);
	}
	//функція збереження списку в файл
	private static void SaveDictionary()
	{
		List<KeyValuePair<string,string>> saveList = _saveData.ToList();
		string saveString="";
		//для кожної пари ключ-значення словника
		foreach(KeyValuePair<string,string> kvp in saveList)
		{
			//додаємо їх в рядок
			saveString += $"{kvp.Key},{kvp.Value}\n";
		}
		//зберігаємо рядок в файл
		File.WriteAllText(GetSavePath(), saveString);
	}
	//функція відновлення списку з файлу
	private static void RestoreDictionary()
	{
		//якщо файлу не існує, створюємо його
		if (!File.Exists(GetSavePath()))
		{
			File.Create(GetSavePath());
		}
		//читаємо дані з файлу
		var save = File.ReadAllLines(GetSavePath());
		//очищаємо список
		_saveData.Clear();
		foreach(string line in save)
		{
			//додаємо кожну пару ключ-значення в словник
			var kvp = line.Split(",");
			_saveData.Add(kvp[0], kvp[1]);
		}
	}
	//фунція отримання шляху файлу збереження
	public static string GetSavePath()
	{
		return Application.persistentDataPath + "/save.xdd";
	}

}
//клас який є інтерфейсом між грою та файлом збереження
public static class SaveData
{
	public static int Level { get { return SaveSystem.GetInteger("level"); } set { SaveSystem.SetInteger("level", value); } }
}
