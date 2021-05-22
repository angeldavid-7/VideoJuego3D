using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorAscensor : MonoBehaviour
{
    public bool upHit = false;
    
   // Asensor asensor = new Asensor();


    void Update()
    {
        if (upHit == true)
        {
           
            Asensor.up = true;


        }
    }

}
