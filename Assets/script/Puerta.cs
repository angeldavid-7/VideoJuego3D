using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public float speed = 0;

    public Vector3 direction;


    public bool puedeAbrir;

    void Moverse()
    {

        gameObject.transform.Translate(0, 0, 0);

    }
     
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {


        if (puedeAbrir == true )
        {
           
                
                Moverse();
                
            
        }
    }
    void OnTriggerStay(Collider other)
    {
#pragma warning disable CS0642 // Posible instrucción vacía errónea
        if (other.gameObject.tag == "player") ;
#pragma warning restore CS0642 // Posible instrucción vacía errónea
        
       PresionarTab.aparecer = true;
        puedeAbrir = true;
    }

     void OnTriggerExit(Collider other)
    {
#pragma warning disable CS0642 // Posible instrucción vacía errónea
        if (other.gameObject.tag == "player") ;
#pragma warning restore CS0642 // Posible instrucción vacía errónea
        PresionarTab.aparecer = false;
        puedeAbrir = false;
        
    }
}
//(0.34375f, 2.6f, 9)