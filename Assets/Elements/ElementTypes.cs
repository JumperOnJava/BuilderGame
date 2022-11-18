using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Типи елементів
public enum ElementType
{
    Empty,//Пустий елемент
    Default,//Каркас
    Engine,//Двигун (не використовується)
    Light,//Фоторезистор
    Laser,//Лазер (не використовується)
    Wheel,//Колесо з двигуном
	Core,//Ядро (не використовується)
	Battery,//Акумулятор
	Propeller,//Пропеллер (не використовується?)
	BasicWheel,//Колесо без двигуна (не використовується)
}

//Повороти елементів, 
public enum ElementRotation
{
    R_0_Degrees=0,
    R_90_Degrees=1,
    R_180_Degrees=2,
    R_270_Degrees=3,
}
//Види елементів за іх роллю в схемі
public enum CircuitElement
{
	None, //Не є елементом схеми
	Sensor, //Датчики
	Engine //Двигун/інший елемент який використовує електрику
}

