using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
//Клас який зберігає дані про певний тип елементу
[CreateAssetMenu(fileName = "ElementInfo", menuName = "Element Info")]
public class ElementInfo : ScriptableObject
{
    //Спрайт в редакторі
    public Sprite Sprite;
    //Назва в списку в редакторі
    public string Name;
    //Шаблон Елементу
    public GameObject Prefab;
    //Роль елемента в Схемі
	  public CircuitElement CircuitElement = CircuitElement.None;
	  //Чи можна повернути цей вид елементу
    public bool Rotates;
    [Space]
    [Header("Connections")]

    //З яких сторін елемент може скріплюватися з іншими елементами
    [SerializeField]
    private bool ConnectsUp = true;
    [SerializeField]
    private bool ConnectsDown = true;
    [SerializeField]
    private bool ConnectsLeft = true;
    [SerializeField]
    private bool ConnectsRight = true;
    public bool[] GetConnections() { return new bool[] { ConnectsUp, ConnectsLeft, ConnectsDown, ConnectsRight }; }

}
