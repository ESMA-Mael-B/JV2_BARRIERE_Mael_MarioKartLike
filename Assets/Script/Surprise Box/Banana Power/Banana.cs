using System.Collections;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarController player = other.GetComponent<CarController>();
            if (player != null)
            {
                StartCoroutine(FreezePlayer(player));
            }
        }
    }

    private IEnumerator FreezePlayer(CarController player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 originalVelocity = rb.velocity;
            rb.velocity = Vector3.zero; 
            player.enabled = false; 

            yield return new WaitForSeconds(2f); 

            player.enabled = true; 
            rb.velocity = originalVelocity; 
        }

        Destroy(gameObject); 
    }
}

