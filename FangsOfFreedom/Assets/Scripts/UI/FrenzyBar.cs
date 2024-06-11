using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FrenzyBar : MonoBehaviour
{
    [SerializeField] private Slider frenzySlider;
    public float frenzyReduction;
    [SerializeField] private float currentFrenzy;

    private void FixedUpdate()
    {
        if (frenzySlider.value <= 0)
        {
            frenzySlider.value = 0;
            return;
        }
        frenzySlider.value = currentFrenzy;
        currentFrenzy -= frenzyReduction * Time.deltaTime;
    }
    
    public void UpdateSlider (float newValue)
    {
        currentFrenzy = newValue;
    }
}
