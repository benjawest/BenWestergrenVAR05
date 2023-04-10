using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoPositionManager : MonoBehaviour
{
    public Transform headTransform; // Reference to the VR rig's head transform
    public float offsetFromHead = 0.5f;

    void Update()
    {
        // Set the position of this game object to be below the VR rig's head
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - offsetFromHead, headTransform.position.z);
    }
}
