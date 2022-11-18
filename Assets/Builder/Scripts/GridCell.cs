using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Клас який зберігає інформацію про вид елементу в контейнері
public struct GridCell
{
    //пуста комірка
	public static GridCell EmptyCell;
	//інформація про тип елементу в контейнері
    private ElementInfo Info;
    public ElementInfo GetInfo()
    {
        return Info;
    }
    public GridCell(ElementType type,ElementInfo info)
    {
        //присвоюємо комірці необхідний тип та інформацію про нього, встановлюємо нульовий поворот, якщо тип елементу пустий то копіюємо дані з пустої комірки
		Type = type;
        Info = info;
		_rotation = ElementRotation.R_0_Degrees;
		if (type == ElementType.Empty)
			EmptyCell = this;
	}
    public bool ShouldConnect(int direction)
    {
        //перевірка того чи буде цей об'єкт з'єднуватися з кожною стороною
        if (GetInfo() == null)
            return false;
        //отримуємо інформацію про можливість з'єднання за кутом який дорівнює
        //остачі від ділення на 4 (кута повороту доданого до кута сторони перевірки з'єднання)
        //кут повороту показується цілим числом від 0 до 3 включно (0, 90, 180, 270 градусів)
        return GetInfo().GetConnections()[((int)Rotation + direction) % 4];
    }
    public ElementType Type;
    //отримання кута повороту
    public ElementRotation Rotation
    {
        get
        {
            //якщо цей тип елементу обертається то повертаєио його поворот інакше повертаємо кут повороту 0;
            if(GetInfo().Rotates)
            {
                return _rotation;
            }
            else
            {
                return ElementRotation.R_0_Degrees;
            }
        }
        set
        {
            _rotation = value;
        }
    }
    private ElementRotation _rotation;
	
}

