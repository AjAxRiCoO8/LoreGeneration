using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(LoreManager))]
public class LoreManagerEditor : Editor
{

    enum displayFieldType { DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields }
    displayFieldType DisplayFieldType;

    LoreManager t;
    SerializedObject GetTarget;
    SerializedProperty rules;
    SerializedProperty init;
    int ListSize = 0;

    void OnEnable()
    {
        t = (LoreManager)target;
        GetTarget = new SerializedObject(t);
        rules = GetTarget.FindProperty("rules"); // Find the List in our script and create a refrence of it
        init = GetTarget.FindProperty("init");
    }

    public override void OnInspectorGUI()
    {

        base.DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Update our list

        GetTarget.Update();

        //Choose how to display the list<> Example purposes only
        DisplayFieldType = displayFieldType.DisplayAsCustomizableGUIFields;

        /*
        EditorGUILayout.LabelField("Define the list size with a number");
        ListSize = ThisList.arraySize;
        ListSize = EditorGUILayout.IntField("List Size", ListSize);

        if (ListSize != ThisList.arraySize)
        {
            while (ListSize > ThisList.arraySize)
            {
                ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
            }
            while (ListSize < ThisList.arraySize)
            {
                ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Or");
        EditorGUILayout.Space();
        EditorGUILayout.Space();

    */

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //Display our list to the inspector window

        for (int i = 0; i < rules.arraySize; i++)
        {
            SerializedProperty MyListRef = rules.GetArrayElementAtIndex(i);
            SerializedProperty ruleStory = MyListRef.FindPropertyRelative("ruleStory");
            SerializedProperty consumedProperties = MyListRef.FindPropertyRelative("consumedProperties");
            SerializedProperty producedProperties = MyListRef.FindPropertyRelative("producedProperties");


            // Display the property fields in two ways.

            if (DisplayFieldType == 0)
            {// Choose to display automatic or custom field types. This is only for example to help display automatic and custom fields.
                //1. Automatic, No customization <-- Choose me I'm automatic and easy to setup
                EditorGUILayout.LabelField("Automatic Field By Property Type");

                // Array fields with remove at index
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Array Fields");

                if (GUILayout.Button("Add New Index", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
                {
                    consumedProperties.InsertArrayElementAtIndex(consumedProperties.arraySize);
                    consumedProperties.GetArrayElementAtIndex(consumedProperties.arraySize - 1).intValue = 0;
                }

                for (int a = 0; a < consumedProperties.arraySize; a++)
                {
                    EditorGUILayout.PropertyField(consumedProperties.GetArrayElementAtIndex(a));
                    if (GUILayout.Button("Remove  (" + a.ToString() + ")", GUILayout.MaxWidth(100), GUILayout.MaxHeight(15)))
                    {
                        consumedProperties.DeleteArrayElementAtIndex(a);
                    }
                }

                // Array fields with remove at index
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Array Fields");

                if (GUILayout.Button("Add New Index", GUILayout.MaxWidth(130), GUILayout.MaxHeight(20)))
                {
                    producedProperties.InsertArrayElementAtIndex(producedProperties.arraySize);
                    producedProperties.GetArrayElementAtIndex(producedProperties.arraySize - 1).intValue = 0;
                }

                for (int a = 0; a < producedProperties.arraySize; a++)
                {
                    EditorGUILayout.PropertyField(producedProperties.GetArrayElementAtIndex(a));
                    if (GUILayout.Button("Remove  (" + a.ToString() + ")", GUILayout.MaxWidth(100), GUILayout.MaxHeight(15)))
                    {
                        producedProperties.DeleteArrayElementAtIndex(a);
                    }
                }
            }
            else
            {
                //Or

                //2 : Full custom GUI Layout <-- Choose me I can be fully customized with GUI options.
                EditorGUILayout.LabelField("Rule #" + (i).ToString());

                // Array fields with remove at index
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rule story", GUILayout.MaxWidth(80));
                ruleStory.stringValue = EditorGUILayout.TextField("", ruleStory.stringValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("Consumed Properties");

                if (GUILayout.Button("Add New Consumed Property", GUILayout.MaxWidth(200), GUILayout.MaxHeight(20)))
                {
                    consumedProperties.InsertArrayElementAtIndex(consumedProperties.arraySize);
                    consumedProperties.GetArrayElementAtIndex(consumedProperties.arraySize - 1).intValue = 0;
                }

                for (int a = 0; a < consumedProperties.arraySize; a++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Actor (" + a.ToString() + ")", GUILayout.MaxWidth(80));
              //      consumedProperties.GetArrayElementAtIndex(a).intValue = EditorGUILayout.IntField("", consumedProperties.GetArrayElementAtIndex(a).intValue, GUILayout.MaxWidth(100));
                    consumedProperties.GetArrayElementAtIndex(a).intValue = EditorGUILayout.Popup("", consumedProperties.GetArrayElementAtIndex(a).intValue, t.Actors.ToArray(), GUILayout.MaxWidth(150));
                    if (GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(15)))
                    {
                        consumedProperties.DeleteArrayElementAtIndex(a);
                    }
                    EditorGUILayout.EndHorizontal();
                }

                // Array fields with remove at index
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Produced Properties");

                if (GUILayout.Button("Add New Produced Property", GUILayout.MaxWidth(200), GUILayout.MaxHeight(20)))
                {
                    producedProperties.InsertArrayElementAtIndex(producedProperties.arraySize);
                    producedProperties.GetArrayElementAtIndex(producedProperties.arraySize - 1).intValue = 0;
                }

                for (int a = 0; a < producedProperties.arraySize; a++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Actor (" + a.ToString() + ")", GUILayout.MaxWidth(80));
                    //      consumedProperties.GetArrayElementAtIndex(a).intValue = EditorGUILayout.IntField("", consumedProperties.GetArrayElementAtIndex(a).intValue, GUILayout.MaxWidth(100));
                    producedProperties.GetArrayElementAtIndex(a).intValue = EditorGUILayout.Popup("", producedProperties.GetArrayElementAtIndex(a).intValue, t.Actors.ToArray(), GUILayout.MaxWidth(150));
                    if (GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(15)))
                    {
                        producedProperties.DeleteArrayElementAtIndex(a);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Space();

            //Remove this index from the List
            if (GUILayout.Button("Remove Rule #" + (i).ToString()))
            {
                rules.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        //Or add a new item to the List<> with a button
        EditorGUILayout.LabelField("Add a new rule with a button");

        if (GUILayout.Button("Add New"))
        {
            t.Rules.Add(new LoreRule());
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Set Init list
        EditorGUILayout.LabelField("Initial Story State");
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        for (int i = 0; i < init.arraySize; i++)
        {


            // Display the property fields in two ways.

            if (DisplayFieldType == 0)
            {// Choose to display automatic or custom field types. This is only for example to help display automatic and custom fields.
                //1. Automatic, No customization <-- Choose me I'm automatic and easy to setup

            }
            else
            {
                //Or
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Actor (" + i.ToString() + ")", GUILayout.MaxWidth(80));
                init.GetArrayElementAtIndex(i).intValue = EditorGUILayout.Popup("", init.GetArrayElementAtIndex(i).intValue, t.Actors.ToArray(), GUILayout.MaxWidth(150));
                if (GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(15)))
                {
                    init.DeleteArrayElementAtIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        //Or add a new item to the List<> with a button
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.MaxWidth(80));
        if (GUILayout.Button("Add new", GUILayout.MaxWidth(150)))
        {
            t.Init.Add(0);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        //Apply the changes to our list
        GetTarget.ApplyModifiedProperties();
    }
}