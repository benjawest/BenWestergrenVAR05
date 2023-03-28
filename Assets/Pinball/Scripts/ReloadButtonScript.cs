using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadButtonScript : MonoBehaviour
{
    public float pushHeight = 0.1f;
    public float returnSpeed = 1.0f;

    private Vector3 initialPosition;
    private bool buttonDown = false;
    private Rigidbody buttonRigidbody;

    private void Start()
    {
        initialPosition = transform.position;
        buttonRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If button is pushed past threshold, and button not already down
        if (transform.position.y < initialPosition.y - pushHeight && !buttonDown)
        {
            buttonDown = true;
            buttonRigidbody.isKinematic = true;

            // Get reference to VR Rig object
            GameObject vrRig = GameObject.Find("VR Rig");

            // Get reference to SceneReloader script on VR Rig
            SceneReloader sceneReloader = vrRig.GetComponent<SceneReloader>();

            // Call ReloadScene method on SceneReloader
            sceneReloader.ReloadScene();

            StartCoroutine(ReturnButton());
        }
        else if (transform.position.y > initialPosition.y)
        {
            // If button has been moved higher than initial position, move it back down
            transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
            buttonRigidbody.velocity = Vector3.zero;
        }
    }

    IEnumerator ReturnButton()
    {
        // Move button back to starting spot using Lerp
        float t = 0.0f;
        Vector3 startPosition = transform.position;
        while (t < 1.0f)
        {
            t += Time.deltaTime * returnSpeed;
            transform.position = Vector3.Lerp(startPosition, initialPosition, t);
            yield return null;
        }
        buttonDown = false;
        buttonRigidbody.isKinematic = false;
    }
}
