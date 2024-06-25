using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem rageParticles;

    private void OnEnable()
    {
        RageBar.OnBerserkExtraDmg += UpdateParticles;
    }

    private void OnDisable()
    {
        RageBar.OnBerserkExtraDmg += UpdateParticles;
    }

    private void UpdateParticles(float floatBool)
    {
        if(floatBool == 0)
        {
            rageParticles.Stop();
            rageParticles.gameObject.SetActive(false);
        }
        else
        {
            rageParticles.gameObject.SetActive(true);
            rageParticles.Play();
        }
    }
}
