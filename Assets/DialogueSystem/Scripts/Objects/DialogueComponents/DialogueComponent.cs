using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

// Tbh I would prefer to use polymorphism on a DialogueComponent but Unity makes that difficult within the editor, so I'm using this DialogueComponentType enum
[System.Serializable]
[ExecuteInEditMode]
public class DialogueComponent
{
    public enum NamePlateAlignment
    {
        Left,
        Center,
        Right,
        None
    }

    // All types
    public DialogueComponentType type;
    public float delay;

    // type = 1 / Text
    public string speakerName;
    public string textContent;
    public NamePlateAlignment namePlateAlignment;

    // type = 2 / Image
    public Sprite image;
    public int imageNumber;

    // type = 3 / Animation
    //public string animationName;

    // type = 4 / SFX
    public AudioClip soundEffect; // also used in Text

    // type = 5 / MusicChange
    public AudioClip song;

    /*
    public static DialogueComponent DialogueComponentFromJsonObject(JObject o)
    {
        DialogueComponent dc = new DialogueComponent();
        dc.type = (DialogueComponentType)System.Enum.Parse(typeof(DialogueComponentType), (string)o.Property("Type").Value);
        dc.delay = o.Property("Delay") != null ? (float)o.Property("Delay").Value : 0f;

        if (dc.type == DialogueComponentType.Text)
        {
            dc.speakerName = (string)o.Property("SpeakerName").Value;
            dc.textContent = (string)o.Property("TextContent").Value;
            dc.namePlateAlign = (string)o.Property("NamePlateAlign") != null ? (string)o.Property("NamePlateAlign").Value : "Left";
            dc.soundEffect = o.Property("SoundEffect") != null ? (string)o.Property("SoundEffect").Value : "";
        }
        else if (dc.type == DialogueComponentType.CharacterChange)
        {
            dc.characterName = (string)o.Property("CharacterName").Value;
        }
        else if (dc.type == DialogueComponentType.Animation)
        {
            dc.animationName = (string)o.Property("AnimationName").Value;
            // These are overloaded for other purposes
            dc.speakerName = o.Property("SpeakerName") != null ? (string)o.Property("SpeakerName").Value : "";
            dc.textContent = o.Property("TextContent") != null ? (string)o.Property("TextContent").Value : "";
        }
        else if (dc.type == DialogueComponentType.SFX)
        {
            dc.soundEffect = (string)o.Property("SoundEffect").Value;
        }
        else if (dc.type == DialogueComponentType.MusicChange)
        {
            dc.songName = (string)o.Property("SongName").Value;
        }

        return dc;
    }
    */
}
