using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoreActor))]
public class LoreActorEditor : Editor {

    public override void OnInspectorGUI()
    {
        LoreActor actor = (LoreActor)target;

        actor.name = EditorGUILayout.Popup("Name: ", actor.name, actor.loreManager.Actors.ToArray());

        actor.state = EditorGUILayout.Popup("State: ", actor.state, actor.loreManager.States.ToArray());

    }
}
