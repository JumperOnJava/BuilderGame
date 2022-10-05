using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    public List<List<GridCell>> Data; 
    public GridData(int height,int width,AllElementsInfo info)
    {
        Data = new List<List<GridCell>>(height);
        for(int i = 0; i < height; i++)
        {
            Data.Add(new List<GridCell>(width));
            for(int j = 0; j < width; j++)
            {
                var newcell = new GridCell(ElementType.Empty,info);
                Data[i].Add(newcell);
            }
        }
    }
}
public class GridCell
{
    private AllElementsInfo Allinfo;
    public ElementInfo GetInfo()
    {
        return Allinfo.GetInfo(Type);
    }
    public GridCell(ElementType type,AllElementsInfo info)
    {
        Type = type;
        Allinfo = info;
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

