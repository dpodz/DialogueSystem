using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Configuration

    public AudioSource sfxPlayer;
    public AudioSource musicPlayer;

    public GameObject dialogueWindow;

    public GameObject nameplateLeft;
    public GameObject nameplateCenter;
    public GameObject nameplateRight;

    public GameObject dialogueText;

    public Button continueButton;

    public GameObject screenBlocker;

    public Image[] images;

    public List<Dialogue> dialogues;


    // Messaging system
    public delegate void OnSystemMessage(string message);
    public static event OnSystemMessage onSystemMessage;


    // Dialogue Running

    private bool isDialogueRunning = false;
    private Dialogue currentDialogue;
    private Queue<DialogueComponent> dialogueComponentQueue;
    private Queue<Dialogue> queuedDialogues;

    private Coroutine typingCoroutine;
    private Coroutine animCoroutine;

    // Public functions

    public void StartDialogue(string dialogueName)
    {
        var dialogue = dialogues.Find(d => d.name == dialogueName);

        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueRunning) // might have to lock this in case of race conditions
        {
            queuedDialogues.Enqueue(dialogue);
        }
        isDialogueRunning = true;

        continueButton.gameObject.SetActive(true);
        screenBlocker.SetActive(true);
        Animator dialogueWindowAnimator = dialogueWindow.GetComponent<Animator>();
        if (dialogueWindowAnimator != null)
            dialogueWindowAnimator.SetBool("open", true);

        dialogueComponentQueue.Clear();
        currentDialogue = dialogue;

        foreach (DialogueComponent dc in dialogue.content)
        {
            dialogueComponentQueue.Enqueue(dc);
        }

        DisplayNextComponent();
    }

    public void ListenToSystemMessages(OnSystemMessage listener)
    {
        onSystemMessage += listener;
    }

    // General Private Functions

    private void Start()
    {
        continueButton.onClick.AddListener(DisplayNextComponent);
    }

    private void OnDisable()
    {
        onSystemMessage = null;
    }

    // Dialogue Private Functions

    private void EndDialogue()
    {
        continueButton.gameObject.SetActive(false);
        screenBlocker.SetActive(false);

        Animator dialogueWindowAnimator = dialogueWindow.GetComponent<Animator>();
        if (dialogueWindowAnimator != null)
            dialogueWindowAnimator.SetBool("open", false);
        
        isDialogueRunning = false;

        currentDialogue = null;
    }

    private void DisplayNextComponent()
    {
        if (dialogueComponentQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Handle the component based on type
        DialogueComponent dialogueComponent = dialogueComponentQueue.Dequeue();
        // Add to history log
        AddToLog(dialogueComponent);

        if (dialogueComponent.type == DialogueComponentType.Text)
        {
            DeactivateNameplates();
            if (dialogueComponent.namePlateAlignment == DialogueComponent.NamePlateAlignment.Left)
            {
                nameplateLeft.SetActive(true);
                TrySetNameplateText(nameplateLeft, dialogueComponent.speakerName);
            }
            else if (dialogueComponent.namePlateAlignment == DialogueComponent.NamePlateAlignment.Right)
            {
                nameplateRight.SetActive(true);
                TrySetNameplateText(nameplateRight, dialogueComponent.speakerName);
            }
            else if (dialogueComponent.namePlateAlignment == DialogueComponent.NamePlateAlignment.Center)
            {
                nameplateCenter.SetActive(true);
                TrySetNameplateText(nameplateCenter, dialogueComponent.speakerName);
            }
            else
            {
                // pass
            }

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence(dialogueComponent.textContent));
        }
        else if (dialogueComponent.type == DialogueComponentType.Image)
        {
            if (animCoroutine != null)
                StopCoroutine(animCoroutine);
            if (dialogueComponent.imageNumber >= images.Length)
            {
                // pass
            }
            else
            {
                animCoroutine = StartCoroutine(TransitionImage(images[dialogueComponent.imageNumber], dialogueComponent.image));
            }
            DisplayNextComponent();
        }
        /*
        else if (dialogueComponent.type == DialogueComponentType.Animation)
        {
            if (dialogueComponent.animationName == "BlackPanelDisplay")
            {
                animPanelTitleText.text = dialogueComponent.speakerName;
                animPanelSubText.text = dialogueComponent.textContent;
                animPanelAnimator.SetTrigger("BlackPanelDisplay");
            }
            DisplayNextComponent();
        }
        */
        else if (dialogueComponent.type == DialogueComponentType.SFX)
        {
            sfxPlayer.PlayOneShot(dialogueComponent.soundEffect);
            DisplayNextComponent();
        }
        else if (dialogueComponent.type == DialogueComponentType.Music)
        {
            if (dialogueComponent.song == null)
            {
                musicPlayer.Stop();
            }
            else
            {
                musicPlayer.clip = dialogueComponent.song;
                musicPlayer.Play();
            }
            DisplayNextComponent();
        }
        else
        {
            Debug.Log("Given dialogueComponent without type, this is INVALID!");
            DisplayNextComponent();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (dialogueText.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            var textBox = dialogueText.GetComponentInChildren<TextMeshProUGUI>();

            textBox.text = "";
            var subSentence = sentence;
            var nextToType = "";
            while ((nextToType = GetNextCharacterToType(subSentence)) != "")
            {
                textBox.text += nextToType;
                subSentence = subSentence.Substring(nextToType.Length);
                yield return null;
            }
        }
        else if (dialogueText.GetComponentInChildren<Text>() != null){
            var textBox = dialogueText.GetComponentInChildren<Text>();

            textBox.text = "";
            var subSentence = sentence;
            var nextToType = "";
            while ((nextToType = GetNextCharacterToType(subSentence)) != "")
            {
                textBox.text += nextToType;
                subSentence = subSentence.Substring(nextToType.Length);
                yield return null;
            }
        }
    }

    IEnumerator TransitionImage(Image image, Sprite sprite)
    {
        Color colour = new Color(image.color.r, image.color.g, image.color.b, image.color.a);
        while (image.color.a > 0f)
        {
            colour.a = Mathf.Max(0, colour.a - 0.05f);
            image.color = colour;
            yield return new WaitForFixedUpdate();
        }

        image.sprite = sprite;

        while (image.color.a < 0.999f)
        {
            colour.a = Mathf.Min(1f, colour.a + 0.05f);
            image.color = colour;
            yield return new WaitForFixedUpdate();
        }
    }

    // Log
    private void AddToLog(DialogueComponent dc)
    {
        return;
        /*
        // Exit if not adding anything to log
        if (dc.type == DialogueComponentType.Animation)
        {
            // pass
            return;
        }
        else if (dc.type == DialogueComponentType.CharacterChange)
        {
            // pass
            return;
        }
        else if (dc.type == DialogueComponentType.MusicChange)
        {
            // pass
            return;
        }
        else if (dc.type == DialogueComponentType.SFX)
        {
            // pass
            return;
        }

        GameObject newLogEntry = Instantiate(LogEntryPrefab);
        var newLogText = newLogEntry.GetComponent<TextMeshProUGUI>();

        // Assign text based on dc
        if (dc.type == DialogueComponentType.Text)
        {
            newLogText.text = "<b>" + dc.speakerName + "</b><br>";
            newLogText.text += dc.textContent;
        }

        newLogEntry.transform.SetParent(LogGrid.transform);
        newLogEntry.transform.localScale = new Vector3(1, 1, 1);
        //newLogEntry.transform.positio
        newLogEntry.SetActive(true);
        */
    }

    // Other Helper Functions

    private void DeactivateNameplates()
    {
        nameplateLeft.SetActive(false);
        nameplateCenter.SetActive(false);
        nameplateRight.SetActive(false);
    }

    private void TrySetNameplateText(GameObject nameplate, string text)
    {
        if (nameplate.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            nameplate.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
        else if (nameplate.GetComponentInChildren<Text>() != null)
        {
            nameplate.GetComponentInChildren<Text>().text = text;
        }
    }

    private string GetNextCharacterToType(string text)
    {
        if (text == "")
        {
            return "";
        }
        else if (text.Substring(0, 1) == "<" && text.Contains(">"))
        {
            return text.Substring(0, text.IndexOf(">") + 2);
        }
        else //( text.Substring(0) != "<")
        {
            return text.Substring(0, 1);
        }
    }

}
