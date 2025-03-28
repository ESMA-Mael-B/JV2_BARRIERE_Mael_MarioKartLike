using UnityEngine;

public class SurpriseBoxBanana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarController car = other.GetComponent<CarController>();
            if (car != null)
            {
                car.ReceiveBanana();
                gameObject.SetActive(false);
            }
        }
    }
}
