using UnityEngine;

public class BoostZone : MonoBehaviour
{
    [SerializeField] private float boostSpeed = 6f;
    [SerializeField] private float boostDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarController player = other.GetComponent<CarController>();
            if (player != null)
            {
                player.ApplyTemporaryBoost(boostSpeed, boostDuration);
            }
        }
    }
}
