using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RefuelTorch : NetworkBehaviour {

    public Light torchLight;

    [SyncVar(hook = "UpdateTorch")]
    public float energy = 300;
    public float torchLightRange = 10;

    private float maxEnergy;
    private float percentEnergy;

    public void Start()
    {
        maxEnergy = energy;
        percentEnergy = 100;
        torchLight.range = torchLightRange / 100 * percentEnergy;
    }
		
    public float DrainTorch()
    {
        if (energy > 0)
        {

            energy-=Time.deltaTime * 20;

            percentEnergy = 100 - (maxEnergy - energy) / (maxEnergy / 100);

            //Debug.Log((maxEnergy - energy) / (maxEnergy / 100));
            
            torchLight.range = torchLightRange / 100 * percentEnergy;

            GetComponent<UpdateTorchParticle>().ParticleUpdate(percentEnergy);

            return 1;
            
            
        }
        else
        {
            return 0;
        }
    }

	void UpdateTorch(float energy) {
		if (energy > 0)
		{

            torchLight.range = torchLightRange / 100 * percentEnergy;
        }
	}
}
