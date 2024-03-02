using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeath : MonoBehaviour
{
    public EnemyScript enemyScript;

    private void OnParticleSystemStopped()
    {
        enemyScript.ParticleSystemStopped();
    }
}
