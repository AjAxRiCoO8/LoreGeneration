using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CustomNetworkTransform : NetworkTransform {

    // can help with avoiding unity bug.
	public override void OnDeserialize (NetworkReader reader, bool initialState)
    {
        base.OnDeserialize (reader, initialState);
	}

}
