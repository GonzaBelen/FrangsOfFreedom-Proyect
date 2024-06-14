using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HungerController : MonoBehaviour
{
    private Combos combos;
    [SerializeField] private HungerBar hungerBar;
    private float hunger = 100;
    private float hungerReduction = 10;
    private float timeToReduceHunger = 5;
    private float currentTimeToReduceHunger;
    

    private void Start()
    {
        combos = GetComponent<Combos>();
        currentTimeToReduceHunger = timeToReduceHunger;
        hungerBar.UpdateSlider(hunger);
    }

    private void FixedUpdate()
    {
        currentTimeToReduceHunger -= Time.deltaTime;
        if(currentTimeToReduceHunger <= 0)
        {
            hunger -= hungerReduction;
            currentTimeToReduceHunger = timeToReduceHunger;
        }

        if (hunger < 0)
        {
            hunger = 0;
        }

        hungerBar.UpdateSlider(hunger);


        if (hunger <= 0)
        {
            // Aquí puedes agregar la lógica para la muerte por hambre, como cargar una escena de Game Over o reiniciar el nivel.
            // SceneManager.LoadScene("GameOver");
        }
    }

    public void GainHunger(int amount)
    {
        hunger += amount;
        hunger = Mathf.Clamp(hunger, 0, 100);
        combos.Combo();
    }

    public void LightExposing()
    {
        timeToReduceHunger = 2.5f;
    }

    public void FinishLightExposing()
    {
        timeToReduceHunger = 5;
    }
}
