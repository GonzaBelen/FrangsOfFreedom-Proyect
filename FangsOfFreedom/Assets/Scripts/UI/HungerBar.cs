using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private TextMeshProUGUI hungerText;

    private void Update()
    {
        //hungerText.text = "Hambre: " + hungerSlider.value.ToString() + " / 100";
    }
    
    public void UpdateSlider (float newValue)
    {
        hungerSlider.value = newValue;
    }
}
