using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TorchController : NetworkBehaviour {

    public Light torchPrefab;

    // Power van de torch en de snelheid waarmee deze uitdoofd.
	[SyncVar(hook = "UpdateTorchPrefab")]
    public float torchRange;

	[SyncVar(hook = "UpdateTorchPrefab")]
    public float torchIntensitie;

    public float extinguishRate = 0.1f;
    public float refuelSpeed = 13;
    public float decreaseSpeed = 10;


    //wordt gebruikt als de torch weer moet worden bijgevult. 
    private float maxTorchRange;
    private float maxTorchIntensitivity;
    public float torchPower;
    private float lastFrameDecrease;
    private float lastPercentUpdate;

    public void Start()
    {
        maxTorchRange = torchRange;
        torchPower = maxTorchRange;
        maxTorchIntensitivity = torchIntensitie;
        decreaseSpeed *= 1000;
    }

    public void Update()
    {
        TorchEctinguish();
    }
    /// <summary>
    /// Decrease the torch power. 
    /// </summary>
    public void TorchEctinguish()
    {
        if (torchPower > 0f  && torchPower <= maxTorchRange)
        {
            float percent = TorchFuelPercent();

            //decrease the torch power every frame. The rest of the calulations will take place on this number. 
            this.torchPower -= extinguishRate * Time.deltaTime;
            
            //torch Intensitie will be set to maxintensitie, minus the percentage that is burned up.
            this.torchIntensitie = maxTorchIntensitivity - (TorchFuelPercent() * (maxTorchIntensitivity / 100));

            //decrease torch is the amount that wil decrease the torch range divided by the torch decrease rate. 
            float decreaseTorch = ((maxTorchRange - torchPower) - Mathf.Pow((maxTorchRange - torchPower), 0.9999f) * Time.deltaTime);
            this.torchRange -= decreaseTorch / decreaseSpeed;

            // update the UI element of the torch.
            //GetComponent<UIController>().UpdateTorchUI(TorchFuelPercent());

            //Hier worden dinge geUpdate die aleen verandert moeten worden als de torch meer dan 10% veranderd is.
            if (Mathf.Abs(percent - lastPercentUpdate) > 10)
                {
                lastPercentUpdate = percent;
                GetComponent<UpdateTorchParticle>().ParticleUpdate(100 - percent);
                }

        }
        else if (torchPower > maxTorchRange)
        {
            torchPower = maxTorchRange;
        }
        else if (torchPower < 0)
        {
            torchPower = 0;
            GetComponent<UpdateTorchParticle>().ParticleUpdate(0);
        }
			
        UpdateTorchPrefab(0);
    }

    //update the torch range.
	public void UpdateTorchPrefab(float value)
    {
        torchPrefab.range = torchRange;
        torchPrefab.intensity = torchIntensitie + 1;
    }
    /// <summary>
    /// In Deze method wordt het opladen van de torch geregeld.
    /// In de Draintorch method die wordt aangeroepen vanuit de meegegeven bron wordt
    /// gekeken of er nog licht in de bron zit. Als dit het geval is wordt er een 1 terug gestuurd
    /// naar deze functie..
    /// </summary>
    /// <param The Fuel source="torchFuel"></param>
    public void RefuelTorch(GameObject torchFuel)
    {
        if (this.torchRange < this.maxTorchRange && torchPower < maxTorchRange)
        {
            RefuelTorch refuelTorch = torchFuel.GetComponent<RefuelTorch>();
            float fuel = refuelTorch.DrainTorch();
            
            if (fuel == 1)
            {
                //Berekening om te kijken hoeveel er bijgevuld moet worden.(Zit een kleine afwijking in met de orginele waarde. De oorzaak
                //hiervan is omdat de snelheid wordt vermenigvuldigt met de refuelspeed, in werkelijkheid is de waarde iedere frame iets anders,
                //maar zonder het vermenigvuldigen zou het heel lang duren voor de torch vol is. de waardes zijn zo klein dat het voor nu
                // verwaarloosbaar is. 

                float increaseTorch = ((maxTorchRange - torchPower) - Mathf.Pow((maxTorchRange - torchPower), 0.9999f) * Time.deltaTime);
                this.torchRange += (increaseTorch / decreaseSpeed) * refuelSpeed;

                if (this.torchRange < this.maxTorchRange)
                    this.torchPower += (extinguishRate * Time.deltaTime) * refuelSpeed;
                else 
                {
                    this.torchPower = maxTorchRange;
                }
                
                //GetComponent<UIController>().UpdateTorchUI(TorchFuelPercent());

            }
        }

       UpdateTorchPrefab(0);

    }

    public float TorchFuelPercent()
    {
        return (maxTorchRange - torchPower) / (maxTorchRange / 100);
    }

}

