using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using static StaticsVariables;

public class Trailer : MonoBehaviour
{
    [SerializeField] private GameObject[] lights;
    [SerializeField] private GameObject[] torches;
    [SerializeField] private GameObject cof;

    private void Start()
    {
        AnalyticsService.Instance.StartDataCollection();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(true);
            }
            foreach (GameObject torch in torches)
            {
                Animator animator = torch.GetComponent<Animator>();
                animator.Play("Fire");
            }
            Animator animator1 = cof.GetComponent<Animator>();
            animator1.Play("Open");
            SessionData.hasFrenzy = true;
            Frenzy frenzy = GetComponent<Frenzy>();
            frenzy.IsInFrenzy();
        }
    }
}
