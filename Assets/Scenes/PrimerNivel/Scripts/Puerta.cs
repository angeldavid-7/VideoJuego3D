using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public float speed = 2;

    public Vector3 direction;


    public bool puedeAbrir;

    void Moverse()
    {

        gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

    }
     
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey("tab") && puedeAbrir == true )
        {
           
                if (gameObject.transform.position.y <= 2.38)
                {
                Moverse();
                }
            
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {


            PresionarTab.aparecer = true;
            puedeAbrir = true;
        }
    }

     void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {

            PresionarTab.aparecer = false;
            puedeAbrir = false;
        }
        
    }
}
//(0.34375f, 2.6f, 9)