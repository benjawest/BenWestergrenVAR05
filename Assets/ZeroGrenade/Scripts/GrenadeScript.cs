using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float blastRadius = 5f;
    [SerializeField] private float blastForce = 700f;
    public float destructionForceThreshold = 10.0f;
    float countdown;
    bool hasExploded = false;
    public bool pinPulled = false;

    [SerializeField] GameObject explosionEffect;



    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (pinPulled)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0  && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    void Explode()
    {
       // Show effect
       if(explosionEffect != null)
        {
            // Instantiate the game object with the particle system component
            Instantiate(explosionEffect, transform.position, Quaternion.Euler(0, 0, 0));

            
        }

        // Get Nearby objects
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, blastRadius);

        foreach(Collider nearbyOjbect in collidersToDestroy)
        {
            // Call the function AddScore(1) on the script GrenadeGameManager on the game object GameManager
            GameObject.Find("GameManager").GetComponent<GrenadeGameManager>().AddScore(1);
            
            // damage
            Destructable dest = nearbyOjbect.GetComponent<Destructable>();
            if (dest != null)
            {
                dest.Destroy();
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            // Add force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
        }

        // Remove grenade
        Destroy(gameObject);
    }

}
