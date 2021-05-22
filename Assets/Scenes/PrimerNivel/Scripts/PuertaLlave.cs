using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaLlave : MonoBehaviour
{

    public static bool doorKey=false;
    public bool open=true;
    public bool close;
    public float speed = 2;
    public bool inTrigger;
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

    void Moverse()
    {

        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

    }

    

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            
           
                if (doorKey)
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        open = true;
                        
                    }
                }
            

            
        }

        if (open && gameObject.transform.position.y <= 3.7)
        {
            Moverse();
        }

    }

    void OnGUI()
    {
        guiStyle.fontSize = 30; //change the font size
        guiStyle.normal.textColor = Color.white;
        if (inTrigger)
        {
   
            
                if (doorKey)
                {
                    GUI.Box(new Rect(490, 530, 200, 25), "Presiona la tecla Tab para abrir", guiStyle);
                }

                else
                {
                    GUI.Box(new Rect(490, 530, 200, 25), "¡Necesitas una llave!", guiStyle);
                }
            
        }
    }
}
