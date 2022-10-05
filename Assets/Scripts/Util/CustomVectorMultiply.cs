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
}
