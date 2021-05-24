﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaBoton : MonoBehaviour
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

        if (Disparo && gameObject.transform.position.x <= 14.117)
        {
            Moverse();
        }

        void Moverse()
        {

            gameObject.transform.Translate(speed * Time.deltaTime, 0, 0);

        }

    }
}
