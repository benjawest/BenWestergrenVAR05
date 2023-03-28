using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinballManager : MonoBehaviour
{
    public GameObject ballPrefab; // Ball Prefab
    public Transform ballSpawn; // Ball spawn transform

    // Material Swap
    public Material opaqueMaterial;
    public Material transparentMaterial;
    public bool isTransparent = true;
    private GameObject[] coverObjects;


    // Start is called before the first frame update
    void Start()
    {
        // Find all objects with the tag "cover"
        coverObjects = GameObject.FindGameObjectsWithTag("Cover");

        // Loop through all cover objects and set their material to transparent on start
        foreach (GameObject coverObject in coverObjects)
        {
            Renderer renderer = coverObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = transparentMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBall()
    {
        Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);
    }

    public void ToggleCoverTransparency()
    {
        isTransparent = !isTransparent; // Toggle the transparency state
        
        // Loop through all cover objects and swap their material
        foreach (GameObject coverObject in coverObjects)
        {
            Renderer renderer = coverObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (isTransparent)
                {
                    renderer.material = opaqueMaterial;
                }
                else
                {
                    renderer.material = transparentMaterial;
                }
            }
        }

    }

    public void TogglePhysics()
    {
        {
            // Find all rigidbodies with the "Ball" tag and toggle their physics
            Rigidbody[] ballRigidbodies = GameObject.FindGameObjectsWithTag("Ball")
                                                   .Select(b => b.GetComponent<Rigidbody>())
                                                   .Where(rb => rb != null)
                                                   .ToArray();

            foreach (Rigidbody rb in ballRigidbodies)
            {
                rb.isKinematic = !rb.isKinematic;
            }
        }
    }
}
