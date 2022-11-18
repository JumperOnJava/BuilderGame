using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using UnityEngine.SceneManagement;
//Дані про рівень
[CreateAssetMenu(fileName = "levelData", menuName = "Level Data")]
public class BuilderData : ScriptableObject
{
    //Назва сцени рівню
	public string Scene;
    //список елементів рівню
    public UDictionary<ElementType, int> Elements;
   
    //розміри сітки будівництва
    [Header("GridSize")]
    public int GridHeight;
    public int GridWidth;
}