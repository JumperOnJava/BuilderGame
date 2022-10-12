using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridCell
{
	public static GridCell EmptyCell;
	private ElementInfo Info;
    public ElementInfo GetInfo()
    {
        return Info;
    }
    public GridCell(ElementType type,ElementInfo info)
    {
		Type = type;
        Info = info;
		_rotation = ElementRotation.R_0_Degrees;
		if (type == ElementType.Empty)
			EmptyCell = this;
	}
    public bool ShouldConnect(int direction)
    {
        if (GetInfo() == null)
            return false;

        return GetInfo().GetConnections()[((int)Rotation + direction) % 4];
    }
    public ElementType Type;
    public ElementRotation Rotation
    {
        get
        {
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

