using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovTexto : MonoBehaviour
{
    public float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.y <= 313)
        Moverse(); 
    }

    void Moverse()
{

    gameObject.transform.Translate(0, speed * Time.deltaTime, 0);

}

}
