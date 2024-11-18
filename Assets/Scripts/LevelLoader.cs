using System.Collections;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    void Start()
    {


    }

    
    void Update()
    
    {
        //Se pressionar qualquer tecla
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            //{
            //    //Muda a Cena
            StartCoroutine(CarregarFase("Fase1"));
        //}
    }

    IEnumerator CarregarFase(string nomeFase)
    {
        //Iniciar a animação
        transition.SetTrigger("Start");

        //Esperar o tempo de animação
        yield return new WaitForSeconds(transitionTime);

        //Carregar a cena
        SceneManager.LoadScene(nomeFase);
    }

}
