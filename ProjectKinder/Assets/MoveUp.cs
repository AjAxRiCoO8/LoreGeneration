using UnityEngine;
using System.Collections;

public class MoveUp : MonoBehaviour {
	
    public float movementSpeed;

    void FixedUpdate()
    {
        Vector3 pos = transform.localPosition;
        pos.y += movementSpeed;
        pos.z += movementSpeed / 3;
        transform.localPosition = pos;
    }
}
