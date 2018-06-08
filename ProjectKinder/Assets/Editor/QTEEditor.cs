using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.EventSystems;

[CustomEditor(typeof(QTE))]
public class QTEEditor : Editor {

	public override void OnInspectorGUI ()
	{
		QTE qte = (QTE)target;

		#region Type Enum
		// Type
		EditorGUILayout.LabelField ("Type", EditorStyles.boldLabel);
		qte.type = (QTEType)EditorGUILayout.EnumPopup ("Type", qte.type);
		#endregion
		#region Keys Array
		// Keys array
		SerializedProperty keys = serializedObject.FindProperty ("keys");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (keys, true);
		if (EditorGUI.EndChangeCheck ()) {
			serializedObject.ApplyModifiedProperties ();
		}
		#endregion
		EditorGUILayout.Space ();
		#region Start Key
		EditorGUILayout.LabelField("Start key", EditorStyles.boldLabel);
		EditorGUILayout.LabelField ("The button to start the qte event, after it has been initialized");
		qte.startKey = (KeyCode)EditorGUILayout.EnumPopup("Start key", qte.startKey);
		#endregion
		EditorGUILayout.Space ();
		#region Order Enum
		// Order
		EditorGUILayout.LabelField ("Order", EditorStyles.boldLabel);
		qte.order = (QTEOrder)EditorGUILayout.EnumPopup ("Order", qte.order);
		#endregion
		EditorGUILayout.Space ();
		#region Duration
		// Duration
		EditorGUILayout.LabelField ("Duration", EditorStyles.boldLabel);
		qte.hasDuration = EditorGUILayout.Toggle ("Has duration", qte.hasDuration);

		if (qte.hasDuration) {
			qte.duration = EditorGUILayout.FloatField ("Duration", qte.duration);
		}
		#endregion
		EditorGUILayout.Space ();
		#region TargetNumber
		// TargetNumber
		EditorGUILayout.LabelField ("Target Number", EditorStyles.boldLabel);
		qte.hasTargetNumber = EditorGUILayout.Toggle ("Has target number", qte.hasTargetNumber);

		if (qte.hasTargetNumber) {
			qte.targetNumber = EditorGUILayout.IntField ("Target", qte.targetNumber);
			EditorGUILayout.LabelField("TargetCount", qte.TargetCount.ToString());
		}
		#endregion
		EditorGUILayout.Space ();
		#region Cool down Timer
		// CooldownTimer
		EditorGUILayout.LabelField ("Cool down effect", EditorStyles.boldLabel);
		qte.hasCooldownEffect = EditorGUILayout.Toggle ("Has cool down effect", qte.hasCooldownEffect);

		if (qte.hasCooldownEffect) {
			qte.coolDownTimer = EditorGUILayout.FloatField ("Target", qte.coolDownTimer);
			EditorGUILayout.LabelField ("Percentage to go up per second.");
			EditorGUILayout.LabelField ("100% equals to one full key press");
			qte.percentageToGoUp = EditorGUILayout.Slider ("Percentage to go up", qte.percentageToGoUp, 0f, 1000f);

			qte.canExceed = EditorGUILayout.Toggle("Can exceed 100%", qte.canExceed);
		}
		#endregion
		EditorGUILayout.Space ();

		// Inheritance text
		EditorGUILayout.LabelField("A script that intherites from AbstractQTE");
		qte.qteObject = (AbstractQTE)EditorGUILayout.ObjectField("Script", qte.qteObject, typeof(AbstractQTE), true);

		Repaint ();

//		#region Script & Function selection
//		// Get all methods on other MonoBehaviour scripts
//		MonoBehaviour[] scripts = qte.GetComponents<MonoBehaviour> ();
//		var flags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
//
//		string[] scriptNames = new string[scripts.Length + 1];
//		scriptNames [0] = "Choose a script";
//
//		for (int i = 0; i < scripts.Length; i++) {
//			//Debug.Log (i + ": " + scripts [i].name);
//			//Debug.Log ("length: " + scripts.Length);
//			scriptNames [i + 1] = scripts [i].GetType ().ToString ();
//		}
//
//		// success condition script popup
//		EditorGUILayout.LabelField("Function called when success", EditorStyles.boldLabel);
//		EditorGUILayout.LabelField ("Choose a script to pick a function from");
//		qte.funcInfoSuccess.scriptIndex = EditorGUILayout.Popup ("Script", qte.funcInfoSuccess.scriptIndex, scriptNames);
//
//		if (qte.funcInfoSuccess.scriptIndex != 0) {
//
//			// Get all methods, including properties
//			MethodInfo[] info = scripts [qte.funcInfoSuccess.scriptIndex - 1].GetType ().GetMethods (flags);
//			List<MethodInfo> methods = new List<MethodInfo> ();
//
//			// Get rid of properties and save.
//			foreach (var method in info) {
//				if (!method.Name.StartsWith ("set_") && !method.Name.StartsWith ("get_") && method.GetParameters().Length == 0) {
//					methods.Add (method);
//				}
//			}
//
//			string[] methodNames = new string[methods.Count];
//
//			for (int i = 0; i < methods.Count; i++) {
//				methodNames [i] = methods [i].Name;
//			}
//
//			if (methodNames.Length != 0) {
//				// success condition method popup
//				EditorGUILayout.LabelField ("Choose a public function to call when the QTE succeeds, functions with parameters are not available");
//				qte.funcInfoSuccess.funcIndex = EditorGUILayout.Popup ("Method", qte.funcInfoSuccess.funcIndex, methodNames);
//
//				// set Function for success condition
//				//qte.funcForSuccess.script = scripts [qte.funcForSuccess.scriptIndex - 1];
//				//qte.funcForSuccess.function = methods [qte.funcForSuccess.funcIndex];
//			} else {
//				EditorGUILayout.LabelField ("There are no public functions to select.");
//				EditorGUILayout.LabelField ("Define a public function, or use a different script");
//				qte.funcInfoSuccess.funcIndex = -1;
//			}
//
//		}
//
//		EditorGUILayout.Space ();
//
//		// fail condition script popup
//		EditorGUILayout.LabelField("Function called when failed", EditorStyles.boldLabel);
//		EditorGUILayout.LabelField ("Choose a script to pick a function from");
//		qte.funcInfoFail.scriptIndex = EditorGUILayout.Popup("Script", qte.funcInfoFail.scriptIndex, scriptNames);
//
//		if (qte.funcInfoFail.scriptIndex != 0) {
//			
//			// Get all methods, including properties
//			MethodInfo[] info = scripts [qte.funcInfoFail.scriptIndex - 1].GetType ().GetMethods (flags);
//			List<MethodInfo> methods = new List<MethodInfo> ();
//
//			// Get rid of properties and save.
//			foreach (var method in info) {
//				if (!method.Name.StartsWith ("set_") && !method.Name.StartsWith ("get_") && method.GetParameters().Length == 0) {
//					methods.Add (method);
//				}
//			}
//
//			string[] methodNames = new string[methods.Count];
//
//			for (int i = 0; i < methods.Count; i++) {
//				methodNames [i] = methods [i].Name;
//			}
//
//			if (methodNames.Length != 0) {
//				// fail condition method popup
//				EditorGUILayout.LabelField ("Choose a public function to call when the QTE fails, functions with parameters are not available");
//				qte.funcInfoFail.funcIndex = EditorGUILayout.Popup ("Method", qte.funcInfoFail.funcIndex, methodNames);
//
//				// set Function for success condition
//				//qte.funcForFail.script = scripts [qte.funcForFail.scriptIndex - 1];
//				//qte.funcForFail.function = methods [qte.funcForFail.funcIndex];
//			} else {
//				EditorGUILayout.LabelField ("There are no public functions to select.");
//				EditorGUILayout.LabelField ("Define a public function, or use a different script");
//				qte.funcInfoFail.funcIndex = -1;
//			}
//		}
//		#endregion
	}
}