using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CustomMathFunctions
{
	// Перемноженяя кожного елемента вектора на відповідний елемент іншого вектору
	public static Vector3 ComponentMultiply(this Vector3 thisVector, Vector3 vector)
    {
        return new Vector3(thisVector.x * vector.x, thisVector.y * vector.y, thisVector.z * vector.z);
    }
	// Знаходження кута між двома точками
	public static float GetAngleBetween(Vector2 a,Vector2 b)
	{
		Vector2 dir = b - a;
		dir.Normalize();
		return Mathf.Atan2(dir.y,dir.x) * 57.2958f; //функція Atan2 видає кут в радіанах, а 1 радіан - приблизно 57.2958 градусів 
	}
	// Ця функція повертає синус аргументу помноженого на число пі, таким чином функція плавно набуває найбільшого значення (1) при n + 0.5
	// та найменшого при n + 1.5 (-1) (n - парне число), 0 якщо число ціле.
	// Використовується при старті рівня для плавного збільшення/зменшення камери
	public static float Lerp010(float value)
	{
		return Mathf.Sin(value * Mathf.PI);
	}
}
