using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private int totalLaps = 3; 
    private int currentLap = 0;

    [SerializeField] private Text lapText;
    [SerializeField] private Image[] lapImages;

    private void Start()
    {
        if (lapText != null)
        {
            lapText.text = "Tour : 0 / " + totalLaps;
        }

        foreach (Image img in lapImages)
        {
            img.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentLap < totalLaps) 
            {
                currentLap++;

                if (lapText != null)
                {
                    lapText.text = "Tour : " + currentLap + " / " + totalLaps;
                }

                for (int i = 0; i < lapImages.Length; i++)
                {
                    lapImages[i].gameObject.SetActive(i == currentLap - 1);
                }
            }
        }
    }
}
