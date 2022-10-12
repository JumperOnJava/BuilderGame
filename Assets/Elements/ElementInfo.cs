using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementInfo", menuName = "Element Info")]
public class ElementInfo : ScriptableObject
{
    public Sprite Sprite;
    public string Name;
    public GameObject ElementPrefab;
    public GameObject GetPrefab(int rotation)
    {
		return ElementPrefab;   
    }
	public CircuitElement CircuitElement = CircuitElement.None;
	public bool Rotates;
    [Space]
    [Header("Connections")]
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
