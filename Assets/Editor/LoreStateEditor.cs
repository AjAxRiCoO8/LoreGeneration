using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoreState))]
public class LoreStateEditor : Editor {

    public override void OnInspectorGUI()
    {
        LoreState state = (LoreState)target;

        if (state.loreManager == null)
        {
            state.loreManager = EditorGUILayout.ObjectField(state.loreManager, typeof(LoreManager), true) as LoreManager;
        }
        else
        {
            state.Name = EditorGUILayout.Popup("State: ", state.Name, state.loreManager.States.ToArray());
        }
    }
}
