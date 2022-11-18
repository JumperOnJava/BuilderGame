using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//клас який зберігає всю необхідну логіку кожного елементу
public abstract class GenericElement : MonoBehaviour
{
	public virtual void ConnectWith(GameObject connectObject)
	{
		if (connectObject == null)
			return;
		var allJoints = connectObject.GetComponents<FixedJoint2D>();
		//додаємо компонент з'єднання двох фізичних елементів
		var joint = gameObject.AddComponent<FixedJoint2D>();
		//joint.breakTorque = 1000;
		//joint.breakForce = 1000;
		joint.autoConfigureConnectedAnchor = true;
		//як об'єкт до якого приєднується цей об'єкт встановлюємо елемент з агрументу
		joint.connectedBody = connectObject.GetComponent<Rigidbody2D>();

	}
	//об'єкт, до якого повинні приєднуватися інші об'єкти які не є цим елементом
    public virtual GameObject GetJointObject()
    {
        return gameObject;
    }
}
