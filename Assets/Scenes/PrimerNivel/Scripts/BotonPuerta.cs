using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonPuerta : MonoBehaviour
{
    public bool inTrigger;
    public bool open;
    public float x;
    public float y;
    public int sizeF;
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Activar();
        }


    }

    public void Activar()
    {



        if (inTrigger)
        {



            PuertaBoton.activador = true;

        }
        else

        {
            PuertaBoton.activador = false;
        }

    }

    private void OnGUI()
    {


        if (inTrigger)
        {
            guiStyle.fontSize = sizeF; //change the font size
            guiStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(x, y, 200, 25), "Presiona TAB para Activar", guiStyle);
        }

    }
}
