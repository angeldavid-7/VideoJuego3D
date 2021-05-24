using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asensor : MonoBehaviour
{
    public float speed = 2;

    public Vector3 direction;


    public static bool up = false;
    public static bool down = false;



    void moveUp()
    {

        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

    }

    void moveDown()
    {

        gameObject.transform.Translate(0, -speed * Time.deltaTime, 0);

    }

    void Update()
    {
        if (up == true)
        {
            if (gameObject.transform.position.y <= 11)
            {
                moveUp();
            }
            
        }

        if (down == true)
        {
            if (gameObject.transform.position.y >= 3.222987)
            {
                moveDown();
            }

        }

    }


}