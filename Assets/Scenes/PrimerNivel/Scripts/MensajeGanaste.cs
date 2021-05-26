using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MensajeGanaste : MonoBehaviour
{
    public static bool aparecer = false;
    Transiciones transiciones;
    Asensor ascensor;

    private void Start()
    {
        transiciones = GameObject.FindGameObjectWithTag("transicion").GetComponent<Transiciones>();
        ascensor = GameObject.FindGameObjectWithTag("ascensorF").GetComponent<Asensor>();

    }

    private void Update()
    {
        if(aparecer == true)
        {
            ganaste();
            aparecer = false;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aparecer = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aparecer = false;
        }

        
    }

    private void ganaste()
    {  
        transiciones.LoadScene("GameOver");
        Llave.resetllaves = true;
        Asensor.resetAscensor = true;
        Cursor.visible = true;
        PuertaBoton.resetPuerta = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

}
