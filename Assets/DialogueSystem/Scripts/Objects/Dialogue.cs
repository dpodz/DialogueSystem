using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;

[System.Serializable]
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<DialogueComponent> content;

    public bool includeInLog = true;

    //public DialogueManager dialogueManager;

    // Based on triggerType:
    // 1 - OnSceneStart

    // 2 - OnAction

    /*
    public static List<Dialogue> LoadDialoguesFromJsonResource(string jsonResourceName)
    {
        var dialogues = new List<Dialogue>();

        // Parse compoundsJson
        TextAsset dialoguesJson = Resources.Load<TextAsset>("Dialogues/" + jsonResourceName);
        JArray dialoguesArr = JArray.Parse(dialoguesJson.text);

        foreach (var o in dialoguesArr.Children<JObject>())
        {
            Dialogue d = new Dialogue();

            d.name = (string)o.Property("Name").Value;
            d.triggerType = (DialogueTriggerType)System.Enum.Parse(typeof(DialogueTriggerType), (string)o.Property("TriggerType").Value);

            // Trigger Type Info
            if (d.triggerType == DialogueTriggerType.OnSceneStart)
            {
                // Pass
            }
            else if (d.triggerType == DialogueTriggerType.OnCompoundCreate)
            {
                d.compoundTriggers = ((string)o.Property("CompoundTriggers").Value).Split(',').ToList();
            }
            else if (d.triggerType == DialogueTriggerType.OnPresentation)
            {
                d.compoundTriggers = ((string)o.Property("CompoundTriggers").Value).Split(',').ToList();
                d.ratingResult = (int)o.Property("RatingResult").Value;
            }
            else if (d.triggerType == DialogueTriggerType.OnRatingEnd)
            {
                d.ratingTriggers = ((string)o.Property("RatingTriggers").Value).Split(',').ToList();
            }
            else if (d.triggerType == DialogueTriggerType.OnPatienceChange)
            {
                d.patienceTrigger = (int)o.Property("PatienceTrigger").Value;
            }

            // Parsing Content
            JArray contentJArr = (JArray)o.Property("Content").Value;

            d.content = new List<DialogueComponent>();
            foreach (var contentObject in contentJArr.Children<JObject>())
            {
                DialogueComponent dc = DialogueComponent.DialogueComponentFromJsonObject(contentObject);
                d.content.Add(dc);
            }

            dialogues.Add(d);
        }

        return dialogues;
    }

    public static Dialogue GetDefaultPresentationDialogue()
    {
        Dialogue defaultDialogue = new Dialogue();

        defaultDialogue.name = "Default presentation dialogue";
        defaultDialogue.compoundTriggers = new List<string>();
        defaultDialogue.content = new List<DialogueComponent>();
        defaultDialogue.ratingResult = 0;
        defaultDialogue.triggerType = DialogueTriggerType.OnPresentation;

        // add some content
        DialogueComponent dc = new DialogueComponent();
        dc.type = DialogueComponentType.Text;
        dc.speakerName = "MC";
        dc.textContent = "Shoot, that's definitely not what they're looking for..."; // randomize?

        defaultDialogue.content.Add(dc);

        return defaultDialogue;
    }

    public static Dialogue GetDefaultRatingDialogue()
    {
        Dialogue defaultDialogue = new Dialogue();

        defaultDialogue.name = "Default rating dialogue (empty)";
        defaultDialogue.content = new List<DialogueComponent>();
        defaultDialogue.triggerType = DialogueTriggerType.OnRatingEnd;

        return defaultDialogue;
    }
    */
}
