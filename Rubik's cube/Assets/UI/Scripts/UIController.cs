using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    public TextMeshProUGUI countDownMesh;
    public TextMeshProUGUI timerMesh;

    private bool countDown = false;
    private bool timing = false;

    private float countDownValue;
    private float timer;

    private Animator transition;
    private string scene;

    private void Start()
    {
        transition = GetComponent<Animator>();
    }

    private void Update()
    {
        if (countDown)
        {
            InspectTimeCountDown();
        }
        else if (timing)
        {
            SolveTimer();
        }
    }

    public void BLD()
    {
        scene = Scene.BLD;
        Debug.Log("Not Avaliable Yet");
        //Transition();
    }

    public void FinishTransitionAnimation()
    {
        SceneManager.LoadScene(scene);
    }

    public void FreePlay()
    {
        scene = Scene.FP;
        Transition();
    }

    public void MainMenu()
    {
        scene = Scene.MENU;
        Transition();
    }

    public void NormalGame()
    {
        scene = Scene.NORMAL;
        Transition();
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }

    public void ReloadScene()
    {
        scene = SceneManager.GetActiveScene().name;
        Transition();
    }

    public void SettingScene()
    {
        scene = Scene.SETTING;
        Debug.Log("Not Yet");
        //Transition();
    }

    public void StartNormalGame()
    {
        countDownValue = 15f;
        countDown = true;
    }

    public void Transition()
    {
        transition.SetTrigger(Spelling.END);
    }

    private void InspectTimeCountDown()
    {
        countDownValue -= Time.deltaTime;
        if (countDownValue > 0)
        {
            if(countDownValue < 3)
            {
                countDownMesh.color = Color.red;
            }
            else if(countDownValue < 8)
            {
                countDownMesh.color = Color.yellow;
            }
            countDownMesh.text = Mathf.CeilToInt(countDownValue).ToString();
        }
        else if(countDownValue > -2)
        {
            countDownMesh.text = "+2";
        }
        else
        {
            countDownMesh.text = "DNF";
            countDown = false;
        }
    }

    private void SolveTimer()
    {
        timer += Time.deltaTime;
        timerMesh.text = string.Format("{0:00}:{1:00}",
                                        Mathf.FloorToInt(timer / 60),
                                        Mathf.FloorToInt(timer % 60));
    }

    public void StartTimer()
    {
        if (countDown)
        {
            countDown = false;
            transform.GetChild(2).gameObject.SetActive(false);
        }
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
        timing = true;
        timer = 0f;
    }

    public void solved()
    {
        timing = false;
    }

}
