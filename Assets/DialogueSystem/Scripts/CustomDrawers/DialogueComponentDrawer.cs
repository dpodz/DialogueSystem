using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DialogueComponent))]
class DialogueComponentDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var singleLineHeight = EditorGUIUtility.singleLineHeight;

        label.text = label.text.Replace("Element", "");

        EditorGUI.BeginProperty(pos, label, prop);
        
        var type = prop.FindPropertyRelative("type");
        var delay = prop.FindPropertyRelative("delay");

        var dcType = (DialogueComponentType)type.intValue;

        var labelRect = new Rect(pos.x, pos.y,                           pos.width, singleLineHeight);
        var typeRect  = new Rect(pos.x + 30, pos.y,                      pos.width - 30, singleLineHeight);
        //var delayRect = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);

        EditorGUI.LabelField(labelRect, label);
        EditorGUI.PropertyField(typeRect, type, new GUIContent());

        EditorGUI.indentLevel+= 2;

        // Conditional stuff
        if (dcType == DialogueComponentType.Text)
        {
            var namePlateAlignment = prop.FindPropertyRelative("namePlateAlignment");
            var speakerName = prop.FindPropertyRelative("speakerName");
            var textContent = prop.FindPropertyRelative("textContent");
            
            var namePlateAlignmentRect  = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);
            var speakerNameRect         = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);

            var textContentLabelRect    = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);
            var textContentAreaRect     = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight*3);
            var textContentAreaStyle    = GUI.skin.GetStyle("TextArea");
            textContentAreaStyle.richText = true;
            textContentAreaStyle.wordWrap = true;

            EditorGUI.PropertyField(namePlateAlignmentRect, namePlateAlignment);
            EditorGUI.PropertyField(speakerNameRect, speakerName);
            EditorGUI.LabelField(textContentLabelRect, new GUIContent("Text Content"));
            textContent.stringValue = EditorGUI.TextArea(textContentAreaRect, textContent.stringValue, textContentAreaStyle);
        }
        else if (dcType == DialogueComponentType.Image)
        {
            var image = prop.FindPropertyRelative("image");
            var imageNumber = prop.FindPropertyRelative("imageNumber");

            var imageRect = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);
            var imageNumberRect = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);

            EditorGUI.PropertyField(imageRect, image);
            EditorGUI.PropertyField(imageNumberRect, imageNumber);
        }
        else if (dcType == DialogueComponentType.Music)
        {
            var song = prop.FindPropertyRelative("song");

            var songRect = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);

            EditorGUI.PropertyField(songRect, song);
        }
        else if (dcType == DialogueComponentType.SFX)
        {
            var sfx = prop.FindPropertyRelative("soundEffect");

            var sfxRect = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width, singleLineHeight);

            EditorGUI.PropertyField(sfxRect, sfx);
        }

        if (dcType == DialogueComponentType.Music || dcType == DialogueComponentType.SFX || dcType == DialogueComponentType.Image)
        {
            var delayLabelRect  = new Rect(pos.x, (pos.y += singleLineHeight + 2), pos.width* 0.30f, singleLineHeight);
            var delayRect       = new Rect(pos.x + pos.width* 0.30f, pos.y, pos.width* 0.70f, singleLineHeight);

            EditorGUI.LabelField(delayLabelRect, new GUIContent("Delay (s)"));
            delay.floatValue = EditorGUI.Slider(delayRect, delay.floatValue, 0, 20);
        }

        EditorGUI.indentLevel-= 2;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        var type = prop.FindPropertyRelative("type");
        var dcType = (DialogueComponentType)type.intValue;

        if (dcType == DialogueComponentType.Text)
        {
            return EditorGUIUtility.singleLineHeight * 7 + 20;
        }
        else if (dcType == DialogueComponentType.Image)
        {
            return EditorGUIUtility.singleLineHeight * 4 + 20;
        }
        else if (dcType == DialogueComponentType.Music)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 20;
        }
        else if (dcType == DialogueComponentType.SFX)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 20;
        }
        else
        {
            // The constant comes from extra spacing between the fields (2px each)
            return EditorGUIUtility.singleLineHeight * 1 + 4 + 8;
        }
    }


}
