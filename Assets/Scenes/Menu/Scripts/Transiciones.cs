using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transiciones : MonoBehaviour
{

    private Animator transicion;
    // Start is called before the first frame update
    void Start()
    {
        transicion = GetComponent<Animator>();
    }

   public void LoadScene(string scene)
    {
        StartCoroutine(Transiciona(scene));
    }

    public void CerrarJuego()
    {
        StartCoroutine(Salir());
        Application.Quit();
        Debug.Log("Funciona Salir");
    }

    IEnumerator Transiciona(string scene)
    {
        transicion.SetTrigger("Salida");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

    IEnumerator Salir()
    {
        transicion.SetTrigger("Salida");
        yield return new WaitForSeconds(1);
        Application.Quit();
        Debug.Log("Funciona Salir");
    }
}
