using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorAscensor : MonoBehaviour
{
    public bool upHit = false;
    public bool downHit = false;

    // Asensor asensor = new Asensor();


    void Update()
    {
        if (upHit == true)
        {
           
            Asensor.up = true;
            Asensor.down = false;
            upHit = false;

        }

        if (downHit == true)
        {

            Asensor.down = true;
            Asensor.up = false;
            downHit = false;
            

        }
    }

}
