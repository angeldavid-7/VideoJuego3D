using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asensor : MonoBehaviour
{
    public float speed = 2;

    public Vector3 direction;


    public static bool up = false;



    void move()
    {

        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

    }

    void Update()
    {
        if (up == true)
        {
            if (gameObject.transform.position.y <= 11)
            {
                move();
            }
            
        }

    }


}