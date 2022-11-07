using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CustomVectorFunctions
{
    public static Vector3 ComponentMultiply(this Vector3 thisVector, Vector3 vector)
    {
        return new Vector3(thisVector.x * vector.x, thisVector.y * vector.y, thisVector.z * vector.z);
    }
    public static Vector3 ComponentDivide(this Vector3 thisVector, Vector3 vector)
    {
        return new Vector3(thisVector.x * (1/vector.x),thisVector.y * (1/vector.y),thisVector.z * (1/vector.z));
    }
	public static float GetAngleBetween(Vector2 a,Vector2 b)
	{
		Vector2 dir = b - a;
		dir.Normalize();
		return Mathf.Atan2(dir.y,dir.x) * 57.2958f; //функція видає кут в радіанах, а 1 радіан - приблизно 57.2958 градусів 
	}
}
