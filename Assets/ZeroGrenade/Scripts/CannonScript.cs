using UnityEngine;
using UnityEngine.InputSystem;

public class CannonScript : MonoBehaviour
{
    [SerializeField] private float forceAmount = 1000f;
    [SerializeField] private float sphereRadius = 0.5f;
    [SerializeField] private bool listenForInput = true;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (listenForInput && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 2f;
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = spawnPosition;

        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        sphere.AddComponent<SphereCollider>();
        rb.AddForce(mainCamera.transform.forward * forceAmount);

        SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = sphereRadius;
        }
    }
}
