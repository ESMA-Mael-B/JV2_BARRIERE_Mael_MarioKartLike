using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] private float _giantSizeMultiplier = 2f;
    [SerializeField] private float _giantDuration = 5f;
    private bool _isUsed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isUsed)
        {
            _isUsed = true;
            other.GetComponent<CarController>().ReceiveGiantItem(_giantSizeMultiplier, _giantDuration);
            gameObject.SetActive(false);
        }
    }
}
