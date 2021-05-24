using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MensajeGanaste : MonoBehaviour
{
    public static bool aparecer = false;
    Transiciones transiciones;

    private void Start()
    {
        transiciones = GameObject.FindGameObjectWithTag("transicion").GetComponent<Transiciones>();

    }

    private void Update()
    {
        if(aparecer == true)
        {
            ganaste(); 
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

}
