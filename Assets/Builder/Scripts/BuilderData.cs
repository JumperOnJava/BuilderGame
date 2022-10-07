using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "levelData", menuName = "Level Data")]
public class BuilderData : ScriptableObject
{
    public UDictionary<ElementType, int> Elements;
   
    [Header("GridSize")]
    public int GridHeight;
    public int GridWidth;
}