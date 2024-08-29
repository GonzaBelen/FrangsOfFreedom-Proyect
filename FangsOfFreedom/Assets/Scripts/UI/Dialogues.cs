using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using static EventManager;
using static StaticsVariables;
using Unity.Services.Analytics;
using UnityEngine.Video;
using Unity.Services.Core;

public class Dialogues : MonoBehaviour
{
    private GameObject player;
    private Stats stats;
    private PlayerController playerController;
    private Rigidbody2D rb2D;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject portrait;
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialogueMarkController;
    [SerializeField] private GameObject dialogueMarkKeyboard;
    [SerializeField] private GameObject movementBarrier;
    [SerializeField] private GameObject nextDialogue;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;
    [SerializeField] private float typingTime = 0.05f;
    private bool isPlayerInRange;
    private bool didDialogueStart = false;
    private int lineIndex;
    // public UnityEvent DialogueFinished;
    private bool keyboard = false;
    private bool controller = false;
    [SerializeField] private VideoClip tutorialClip;
    [SerializeField] private GameObject videoObject;
    [SerializeField] private GameObject tutorialVideos;
    private bool isInVideo = false;
    
    private void Start()
    {
        UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        player = GameObject.FindWithTag("Player");
        stats = player.gameObject.GetComponent<Stats>();
        playerController = player.gameObject.GetComponent<PlayerController>();
        rb2D = player.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (controller && isPlayerInRange && !SessionData.controllerConnected)
        {
            dialogueMark.SetActive(false);
            dialogueMark = dialogueMarkKeyboard;
            dialogueMark.SetActive(true);
        }

        if (keyboard && isPlayerInRange && SessionData.controllerConnected)
        {
            dialogueMark.SetActive(false);
            dialogueMark = dialogueMarkController;
            dialogueMark.SetActive(true);
        }

        if (SessionData.controllerConnected)
        {
            keyboard = false;
            controller = true;
            dialogueMark = dialogueMarkController;
        } else
        {
            keyboard = true;
            controller = false;
            dialogueMark = dialogueMarkKeyboard;
        }
     
        if (tutorialVideos != null && playerController.changeLine && isInVideo)
        {
            playerController.changeLine = false;
            isInVideo = false;
            tutorialVideos.SetActive(false);
            FinishDialogue();
        }

        if (isInVideo)
        {
            return;
        }

        if (isPlayerInRange && playerController.changeLine)
        {
            if (!didDialogueStart)
            {
                playerController.changeLine = false;
                StartDialogue();
            } else if (dialogueText.text == dialogueLines[lineIndex]  && playerController.changeLine)
            {
                playerController.changeLine = false;
                NextDialogueLine();
            } else if (dialogueText.text != dialogueLines[lineIndex] && playerController.changeLine)
            {
                playerController.changeLine = false;
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
        dialogueMark.SetActive(false);
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
            playerController.isInDialogueRange = true;
            dialogueMark.SetActive(true);
        }    
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerController.isInDialogueRange = false;
            dialogueMark.SetActive(false);
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
            if (tutorialVideos != null)
            {
                isInVideo = true;
                portrait.SetActive(false);
                dialoguePanel.SetActive(false);
                tutorialVideos.SetActive(true);
                VideoChanger videoChanger = videoObject.gameObject.GetComponent<VideoChanger>();
                videoChanger.ChangeVideo(tutorialClip);
                return;
            }     
            FinishDialogue();
        }
    }

    private void FinishDialogue()
    {
        DialogueEvent dialogue = new DialogueEvent
        {
            dialogueTime = SessionData.dialogueTimer,
            dialogueName = gameObject.name.ToString(),
        };

        AnalyticsService.Instance.RecordEvent(dialogue);
        AnalyticsService.Instance.Flush();

        // DialogueFinished?.Invoke();
        playerController.isInDialogue = false;
        playerController.stop = false;
        stats.hungerReduction = 10;
        didDialogueStart = false;
        portrait.SetActive(false);
        dialoguePanel.SetActive(false);
        dialogueMark.SetActive(true);
        DestroyBarrier();
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