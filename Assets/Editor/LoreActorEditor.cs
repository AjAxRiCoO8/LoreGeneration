using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoreActor))]
public class LoreActorEditor : Editor {

    public override void OnInspectorGUI()
    {
        LoreActor actor = (LoreActor)target;

        actor.name = EditorGUILayout.TextField("Name: ", actor.name);

        int selected = EditorGUILayout.Popup("State: ", 0, actor.loreManager.States.ToArray());
    }
}
