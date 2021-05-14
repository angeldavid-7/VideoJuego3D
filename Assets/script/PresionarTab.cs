using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PresionarTab : MonoBehaviour
{
    public static bool aparecer = false;
    public float x;
    public float y;
    public int sizeF;
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable
    

    private void OnGUI()
    {
       

        if (aparecer == true)
        {
            guiStyle.fontSize = sizeF; //change the font size
            guiStyle.normal.textColor = Color.white;
            GUI.Box(new Rect(x, y, 200, 25), "Manten TAB para abrir", guiStyle);
        }
        
    }
}
