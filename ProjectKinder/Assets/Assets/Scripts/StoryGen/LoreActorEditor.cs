using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoreActor))]
public class LoreActorEditor : Editor {

    public override void OnInspectorGUI()
    {
        LoreActor actor = (LoreActor)target;

        if (actor.loreManager == null)
        {
            actor.loreManager = EditorGUILayout.ObjectField(actor.loreManager, typeof(LoreManager), true) as LoreManager;
        }
        else
        {
            actor.Name = EditorGUILayout.Popup("Name: ", actor.Name, actor.loreManager.Actors.ToArray());
        }
    }
}
