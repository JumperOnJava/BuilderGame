using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelElement : GenericElement
{
    [SerializeField]
    private Rigidbody2D _jointWheelHolder;
    public override GameObject GetJointObject()
    {
        return _jointWheelHolder.gameObject;
    }
}
