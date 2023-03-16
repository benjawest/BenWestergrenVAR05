using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VRRig : MonoBehaviour
{

    public Transform head, left, right;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if( XRController.leftHand != null)
        {
            // Update rig transforms

            Vector3 leftPosition = XRController.leftHand.devicePosition.ReadValue();
            Quaternion leftRotation = XRController.leftHand.deviceRotation.ReadValue();
            left.SetPositionAndRotation(leftPosition, leftRotation);
        }

        if (XRController.rightHand != null)
        {
            // Update rig transforms

            Vector3 rightPosition = XRController.rightHand.devicePosition.ReadValue();
            Quaternion rightRotation = XRController.rightHand.deviceRotation.ReadValue();
            left.SetPositionAndRotation(rightPosition, rightRotation);
        }

        //if (XRHMD. != null)
        //{
        //    // Update rig transforms

        //    Vector3 rightPosition = XRController.rightHand.devicePosition.ReadValue();
        //    Quaternion rightRotation = XRController.rightHand.deviceRotation.ReadValue();
        //    left.SetPositionAndRotation(rightPosition, rightRotation);
        //}

    }
}
