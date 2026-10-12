using UnityEngine;
using TMPro;
using System.Collections;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float dialogueDuration = 3.0f;

    private Coroutine dialogueRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideGuide();
        dialoguePanel.SetActive(false);
    }

    public void ShowGuide(string message)
    {
        guideText.gameObject.SetActive(true);
        guideText.text = message;
    }

    public void HideGuide()
    {
        guideText.gameObject.SetActive(false);
    }

    private IEnumerator DialogueRoutine(string npcName, string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = npcName + "\n" + message;

        yield return new WaitForSeconds(dialogueDuration);

        dialoguePanel.SetActive(false);
        dialogueRoutine = null;
    }

    public void ShowDialogue(string npcName, string message)
    {
        if (dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine);
        }

        dialogueRoutine = StartCoroutine(DialogueRoutine(npcName, message));
    }
}