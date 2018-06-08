using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#region enums
public enum QTEType
{
	ButtonMash
}

public enum QTEOrder
{
	Random,
	InOrder,
	Different,
	Ignore
}
#endregion

public class QTE : MonoBehaviour
{

	#region public fields

	public QTEType type;

	public KeyCode[] keys;

	public KeyCode startKey;

	public QTEOrder order;

	public bool hasDuration;
	public float duration;

	public bool hasTargetNumber;
	public int targetNumber;

	public bool hasCooldownEffect;
	public float coolDownTimer;
	public float percentageToGoUp;
	public bool canExceed;

	public AbstractQTE qteObject;

	#endregion

	#region Private fields

	// private fields
	private bool qteIsActive = false;
	private bool qteIsRunning = false;

	private KeyCode lastKeyPressed = KeyCode.None;

	private float durationClock;
	private float durationCountDown;

	private float targetCount;
	private float targetPercentage = 100;
	private float targetSteps;

	// keeps track of the max time in between key presses
	private float coolDownClock;
	private float coolDownCountDown;

	private float percentagePerSecond;

	private bool QTESucceeded;
	private bool QTEFinished;

	private UIController playerUI;

	#endregion

	#region methods

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{		 
		durationClock = duration;
		coolDownClock = coolDownTimer;

		targetCount = targetNumber;
		targetSteps = targetPercentage / targetCount;

		percentagePerSecond = percentageToGoUp;

		if (startKey == KeyCode.None) {
			Debug.LogWarning ("Start key not defined, QTE will start on initialize");
			qteIsRunning = true;
		}
	}
    /*
	void Update ()
	{

		if (qteObject.StartQTE) {
			StartQTE ();
		} else {
			Debug.Log ("HALLLOOO");
		}

		if (qteIsActive) {
			playerUI = qteObject.PlayerObject.GetComponent<UIController> ();
			//playerUI.ActivateQTE ();
			if (keys.Length == 1) {
				playerUI.SetLeftKey (keys [0].ToString ());
			} else if (keys.Length == 2) {
				playerUI.SetLeftKey (keys [0].ToString ());
				playerUI.SetRightKey (keys [1].ToString ());
			}

			if (qteIsRunning) {
				DoQTE ();
				playerUI.UpdateQTEUI (targetPercentage);
				qteObject.PercentageCompleted = 100 - targetPercentage;
			} else {
				if (Input.GetKeyDown (startKey)) {
					qteIsRunning = true;
				}
			}
		} 
	}
    */

	public void StartQTE() {
		qteIsActive = true;
	}

	/// <summary>
	/// Starts the QTE
	/// </summary>
	private void DoQTE ()
	{
		switch (type) {
		case QTEType.ButtonMash:
			DoButtonMash ();
			break;
		default:
			break;
		}
	}

	/// <summary>
	/// Activates QTE in buttonmash mode.
	/// </summary>
	private void DoButtonMash ()
	{
		if (keys.Length == 0) {
			Debug.LogError ("There are no keys to be mashed. Add keys to the array for the Quick Time Event to be used.");
		} else if (keys.Length == 1) {
			order = QTEOrder.Ignore;
		}

		// if order is different
		if (order == QTEOrder.Different) {
			DoDifferent ();
		} else if (order == QTEOrder.Random) {
			// doRandom();
		} else {
			// doInOrder();
		}
	}

	private void DoDifferent ()
	{
		if (!QTEFinished) {

			KeyCode pressedKey;

			if (hasDuration) {
				DoDuration ();
			}

			// check for cooldown effect
			if (hasCooldownEffect) {
				DoCoolDownEffect ();
			}

			if (IsThisInArray (out pressedKey) && IsInputCorrect (pressedKey)) {
				if (hasCooldownEffect) {
					ResetCoolDown ();
				}

				DecreaseTarget ();

				if (QTESucceeded = CheckIfSucceeded ()) {
					QTEFinished = true;
					qteIsRunning = false;
					qteIsActive = false;
					InvokeMethod ();
				}
			}
		}
	}

	private void InvokeMethod() {
		//playerUI.DeactivateQTE ();
		qteIsActive = false;
		qteIsRunning = false;
		if (QTESucceeded) {
			qteObject.SuccessOutcome ();
		} else {
			qteObject.FailOutcome ();
		}
		Reset ();
	}

	/// <summary>
	/// Checks if pressed key is allowed.
	/// </summary>
	/// <returns><c>true</c>, if this in array was ised, <c>false</c> otherwise.</returns>
	/// <param name="pressedKey">Pressed key.</param>
	private bool IsThisInArray (out KeyCode pressedKey)
	{
		foreach (var key in keys) {
			if (Input.GetKeyDown (key)) {
				pressedKey = key;
				return true;
			}
		}
		pressedKey = KeyCode.None;
		return false;			
	}

	/// <summary>
	/// check if inut is correct for the type of order that is defined.
	/// </summary>
	/// <returns><c>true</c>, if input correct was ised, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	private bool IsInputCorrect (KeyCode key)
	{
		switch (order) {
		case QTEOrder.Different:
			return IsKeyDifferent (key);
		default:
			return true;
		}
	}

	/// <summary>
	/// Checks if the key is different then the last one.
	/// </summary>
	/// <returns><c>true</c>, if key different was ised, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	private bool IsKeyDifferent (KeyCode key)
	{
		bool isLastKeyDifferent;
		if (lastKeyPressed == KeyCode.None) {
			isLastKeyDifferent = true;
		} else if (lastKeyPressed != key) {
			isLastKeyDifferent = true;
		} else {
			isLastKeyDifferent = false;
		}
		lastKeyPressed = key;

		return isLastKeyDifferent;		
	}

	/// <summary>
	/// Decreases the target counter.
	/// </summary>
	private void DecreaseTarget ()
	{
		targetCount--;
		targetPercentage -= targetSteps;
	}

	/// <summary>
	/// Keeps track of the duration
	/// </summary>
	private void DoDuration ()
	{
		durationClock -= Time.deltaTime;
		Debug.Log ("Time left: " + durationClock);

		if (durationClock <= 0) {
			QTEFinished = true;
			InvokeMethod ();
			durationClock = duration;
		}
	}

	/// <summary>
	/// Keeps track of the cooldown effect.
	/// </summary>
	private void DoCoolDownEffect ()
	{
		coolDownClock -= Time.deltaTime;

		if (coolDownClock <= 0) {
			if (canExceed || targetCount < targetNumber) {
				targetPercentage += targetSteps * (percentagePerSecond / 100) * Time.deltaTime;
				targetCount = targetPercentage * targetNumber / 100f;
			} else {
				targetCount = targetNumber;
			}
		}
	}

	/// <summary>
	/// Resets the cool down timer.
	/// </summary>
	private void ResetCoolDown ()
	{
		coolDownClock = coolDownTimer;
	}

	/// <summary>
	/// Checks if succeeded.
	/// </summary>
	/// <returns><c>true</c>, if if succeeded was checked, return true, <c>false</c> otherwise, return false.</returns>
	private bool CheckIfSucceeded ()
	{
		if (targetPercentage <= 0) {
			return true;
		}
		return false;
	}

	private void Reset() {
		durationClock = duration;
		coolDownClock = coolDownTimer;

		targetCount = targetNumber;
		targetPercentage = 100;
		targetSteps = targetPercentage / targetCount;

		percentagePerSecond = percentageToGoUp;

		qteIsRunning = false;
		qteIsActive = false;
		QTEFinished = false;
		QTESucceeded = false;
	}
	#endregion

	#region Properties
	public float TargetCount {
		get { return targetCount; }
	}
	#endregion
}

public abstract class AbstractQTE : MonoBehaviour {

	public abstract void SuccessOutcome();
	public abstract void FailOutcome();

	public abstract bool StartQTE {
		get;
	}

	public abstract GameObject PlayerObject {
		get;
	}

	public abstract float PercentageCompleted {
		get;
		set;
	}

}