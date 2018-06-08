using UnityEngine;
using System.Collections;

public class UpdateTorchParticle : MonoBehaviour {

    public ParticleSystem fire;
    public float emmisionRateStart;

    void Start()
    {
        var em = fire.emission;
        var rate = em.rate;

        rate.constantMax = emmisionRateStart;
        em.rate = rate;
    }

    /// <summary>
    /// In deze methode wordt de emission rate aangepast voor de particles. Dit houdt in dat ze sneller of langzamer kunnen spawnen. 
    /// </summary>
    public void ParticleUpdate(float percent)
    {
        float emmisionRate = (emmisionRateStart / 100) * percent;

        var em = fire.emission;
        var rate = em.rate;

        rate.constantMax = emmisionRate;
        em.rate = rate;
    }
}
