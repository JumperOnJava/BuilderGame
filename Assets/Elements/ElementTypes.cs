using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Empty,
    Default,
    Engine,
    Light,
    Laser,
    Wheel,
	Core,
	Battery,
	Propeller,
	BasicWheel // Колесо яке використовується для першого рівня і не є електричним елементом
}
public enum ElementRotation
{
    R_0_Degrees=0,
    R_90_Degrees=1,
    R_180_Degrees=2,
    R_270_Degrees=3,
}
public enum CircuitElement
{
	None,
	Sensor,
	Engine
}

