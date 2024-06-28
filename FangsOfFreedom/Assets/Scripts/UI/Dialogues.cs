using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using static EventManager;
using static StaticsVariables;

public class Dialogues : MonoBehaviour
{
    private GameObject player;
    private Stats stats;
    private PlayerController playerController;
    private Rigidbody2D rb2D;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject portrait;
    //[SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject movementBarrier;
    [SerializeField] private GameObject nextDialogue;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private float typingTime = 0.05f;
    private bool isPlayerInRange;
    private bool didDialogueStart = false;
    private int lineIndex;
    
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        stats = player.gameObject.GetComponent<Stats>();
        playerController = player.gameObject.GetComponent<PlayerController>();
        rb2D = player.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            } else if (dialogueText.text == dialogueLines[lineIndex]  && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)))
            {
                NextDialogueLine();
            } else if (dialogueText.text != dialogueLines[lineIndex] && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z)))
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
    }

    private void StartDialogue()
    {
        rb2D.velocity = Vector2.zero;
        rb2D.angularVelocity = 0; 
        playerController.isInDialogue = true;
        playerController.stop = true;
        stats.hungerReduction = 0;
        didDialogueStart = true;
        //dialogueMark.SetActive(false);
        dialoguePanel.SetActive(true);
        portrait.SetActive(true);
        lineIndex = 0;
        StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            //dialogueMark.SetActive(true);
        }    
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            //dialogueMark.SetActive(false);
        }    
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        } else
        {
            DialogueEvent power = new DialogueEvent
            {
                time = SessionData.dialogueTimer,
            };
            playerController.isInDialogue = false;
            playerController.stop = false;
            stats.hungerReduction = 10;
            didDialogueStart = false;
            portrait.SetActive(false);
            dialoguePanel.SetActive(false);
            //dialogueMark.SetActive(true);
            DestroyBarrier();
        }
    }

    public void DestroyBarrier()
    {
        if (movementBarrier != null)
        {
            movementBarrier.SetActive(false);
        }
        gameObject.SetActive(false);
        ActiveNextDialogue();
    }

    private void ActiveNextDialogue()
    {
        if (nextDialogue != null)
        {
            nextDialogue.SetActive(true);
        }    
    }
}