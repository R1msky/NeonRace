using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
   

     void Update()
    {
       
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
