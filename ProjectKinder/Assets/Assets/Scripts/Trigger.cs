using UnityEngine;
using System.Collections;

public abstract class Trigger : MonoBehaviour {
    
	public abstract bool IsClosed {
		get;
        set;
	}
}
