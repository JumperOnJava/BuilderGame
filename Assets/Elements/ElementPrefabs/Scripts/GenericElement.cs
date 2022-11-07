using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public abstract class GenericElement : MonoBehaviour
{
	public virtual void ConnectWith(GameObject connectObject)
	{
		if (connectObject == null)
			return;
		var allJoints = connectObject.GetComponents<FixedJoint2D>();

		var joint = gameObject.AddComponent<FixedJoint2D>();
		//joint.breakTorque = 1000;
		//joint.breakForce = 1000;
		joint.autoConfigureConnectedAnchor = true;
		joint.connectedBody = connectObject.GetComponent<Rigidbody2D>();

	}
    public virtual GameObject GetJointObject()
    {
        return gameObject;
    }
}
