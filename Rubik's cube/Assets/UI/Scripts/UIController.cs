using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public bool ready = false;
    private Animator transition;
    private string scene;

    private void Start()
    {
        transition = GetComponent<Animator>();
    }

    public void NormalGame()
    {
        scene = Scene.NORMAL;
        Transition();
    }

    public void FreePlay()
    {
        scene = Scene.FP;
        Transition();
    }

    public void BLD()
    {
        scene = Scene.BLD;
        Debug.Log("Not Avaliable Yet");
        //Transition();
    }

    public void SettingScene()
    {
        scene = Scene.SETTING;
        Debug.Log("Not Yet");
        //Transition();
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }


    public void Transition()
    {
        transition.SetTrigger(Spelling.END);
    }

    public void FinishTransitionAnimation()
    {
        SceneManager.LoadScene(scene);
    }
}
