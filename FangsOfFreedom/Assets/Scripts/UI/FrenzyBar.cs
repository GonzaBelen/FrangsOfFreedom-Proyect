using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;

public class FrenzyBar : MonoBehaviour
{
    [SerializeField] private Slider frenzySlider;
    public float frenzyReduction;
    public float currentFrenzy;
    private float initialFrenzy = 0;
    public bool beginInFrenzy = false;

    private void Start()
    {
        frenzySlider.value = initialFrenzy;
    }

    private void FixedUpdate()
    {
        if (currentFrenzy <= 0)
        {
            currentFrenzy = 0;
            return;
        }
        if (currentFrenzy >= 100)
        {
            currentFrenzy = 100;
        }
        frenzySlider.value = currentFrenzy;
        currentFrenzy -= frenzyReduction * Time.deltaTime;
    }
    
    public void UpdateSlider (float amount)
    {
        currentFrenzy += amount;
    }
}