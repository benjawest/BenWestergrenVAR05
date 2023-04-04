using UnityEngine;
using UnityEngine.InputSystem;

public class CannonScript : MonoBehaviour
{
    [SerializeField] private float forceAmount = 1000f;
    [SerializeField] private float sphereSize = 0.5f;
    [SerializeField] private bool listenForInput = true;
    [SerializeField] private float grenadeLife = 3f;

    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (listenForInput && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (mainCamera != null)
        {
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 2f;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = spawnPosition;
            sphere.transform.localScale = new Vector3(sphereSize, sphereSize, sphereSize);

            Rigidbody rb = sphere.AddComponent<Rigidbody>();
            sphere.AddComponent<SphereCollider>();
            rb.AddForce(mainCamera.transform.forward * forceAmount);

            SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                sphereCollider.radius = sphereSize;
            }

            Destroy(sphere, grenadeLife); // Destroy the sphere after grenadeLife seconds
        }
    }
}
