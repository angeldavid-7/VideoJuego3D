using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Llave : MonoBehaviour
{
    public static int keycount = 0;
    public int contadorPrueba = 0;
    public static int contador=0;
    public static bool resetllaves = false;
    public Text Llaves;
    public float contllaves=0;
    // Start is called before the first frame update
    void Start()
    {

        contador = 0;
        contadorPrueba = 0;
        keycount = 5;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(resetllaves == true) {
            keycount = 5;
            resetllaves = false;
            PuertaLlave.doorKey = false; 
        }

        contadorPrueba = contador;
        contllaves = keycount;
        
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            contador++;
           
            Update();
            Llaves.text = contadorPrueba.ToString();
        }
    }

     void OnDestroy()
    {
        keycount--;


        if (keycount <= 0)
        {
            PuertaLlave.doorKey = true;
            Debug.Log("GANASTE CRACK");
        }
    }



}
