using UnityEngine;

public class SurpriseBox : MonoBehaviour
{
    [SerializeField] private GameObject _visual;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarController player = other.GetComponent<CarController>();
            if (player != null)
            {
                player.ReceiveBoost();
                Destroy(gameObject);
            }
        }
    }
}