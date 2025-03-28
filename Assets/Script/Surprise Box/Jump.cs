using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private CarController carController;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
           
            carController.EnableJump();
         
            gameObject.SetActive(false);
        }
    }
}