using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MensajeGranada : MonoBehaviour
{
    public static bool aparecer = false;
    public float x;
    public float y;
    public int sizeF;
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aparecer = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aparecer = false;
        }
    }

    private void OnGUI()
    {


        if (aparecer == true)
        {
            guiStyle.fontSize = sizeF; //change the font size
            guiStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(x, y, 200, 25), "¡Impulsate con una Granada!", guiStyle);
        }

    }
}
