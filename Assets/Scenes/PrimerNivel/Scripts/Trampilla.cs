using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampilla : MonoBehaviour
{ 
public static bool doorKey;
public bool open;
public float speed = 2;
public bool inTrigger;
    public static bool activador;

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        inTrigger = true;
    }
}

void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Player"))
    {
        inTrigger = false;
    }
}

    // Update is called once per frame
    void Update()
    {
        Confirmacion(activador);
    }



    public void Confirmacion(bool Disparo)
    {

        if (Disparo)
        {
            Moverse();
        }

        void Moverse()
        {

            var newRot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(79.0f,180f, 0f), Time.deltaTime * 10);
            transform.rotation = newRot;

        }

    }










}
