using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    // The instance of the object you want to keep across scene changes
    private GameObject objectInstance;

    private void Awake()
    {
        // Find the VR Rig object in the scene
        objectInstance = GameObject.Find("VR Rig");

        // If there's no instance of the object, log an error message
        if (objectInstance == null)
        {
            Debug.LogError("VR Rig object not found in the scene.");
            return;
        }

        // Don't destroy the object to keep when a new scene is loaded
        DontDestroyOnLoad(objectInstance);

        // If there's already an instance of the object in the scene, destroy this one
        if (GameObject.Find(objectInstance.name) != gameObject)
        {
            Destroy(gameObject);
        }
    }

    public void ReloadScene()
    {
        // Store the current position and rotation of the VR Rig camera
        Vector3 cameraPosition = objectInstance.transform.position;
        Quaternion cameraRotation = objectInstance.transform.rotation;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Find the VR Rig camera in the new scene and set its position and rotation
        if (objectInstance != null)
        {
            objectInstance.transform.position = cameraPosition;
            objectInstance.transform.rotation = cameraRotation;
        }
    }
}

