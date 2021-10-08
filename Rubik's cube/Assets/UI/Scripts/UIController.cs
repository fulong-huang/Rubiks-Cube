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
    public TextMeshProUGUI summaryMesh;

    private bool countDown = false;
    private bool timing = false;
    private bool p2 = false;
    private bool DNF = false;

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

    // ***************** Transitions *****************
    public void BLD()
    {
        scene = Scene.BLD;
        Transition();
    }

    public void FreePlay()
    {
        scene = Scene.FP;
        countDown = timing = false;
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

    public void Transition()
    {
        transition.SetTrigger(Spelling.END);
    }

    public void FinishTransitionAnimation()
    {
        SceneManager.LoadScene(scene);
    }

    // ***************** End of Transitions *****************



    public void QuitGame()
    {
        Debug.Log("QUITTING");
        Application.Quit();
    }

    public void StartBlindGame() {
        StartTimer();
    }

    public void StartNormalGame()
    {
        countDownValue = 15f;
        countDown = true;
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
            p2 = true;
            countDownMesh.text = "+2";
        }
        else
        {
            DNF = true;
            countDownMesh.text = "DNF";
            countDown = false;
        }
    }

    private void SolveTimer()
    {
        timer += Time.deltaTime;
        timerMesh.text = string.Format("{0:00}:{1:00}.{2:000}",
                                        Mathf.FloorToInt(timer / 60),
                                        Mathf.FloorToInt(timer%60),
                                        Mathf.FloorToInt(timer%1*1000));
    }

    public void StartTimer()
    {
        if (countDown)
        {
            countDown = false;
            transform.GetChild(3).gameObject.SetActive(false);

            countDownMesh.color = Color.white;
            if (DNF)
                timerMesh.color = Color.red;
            else if (p2)
                timerMesh.color = Color.yellow;

        }
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

        DNF = false;
        p2 = false;
        timing = true;
        timer = 0f;
    }

    public void stopTimer()
    {
        timing = false;
    }

    public void solved(char c = ' ')
    {
        if(c != ' ')
        {
            if (c == 'D')
                DNF = true;
            else
                p2 = true;
        }
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        if (DNF)
        {
            summaryMesh.text = "Did not Finished\n" +
                                string.Format("{0:00}:{1:00}.{2:000}",
                                        Mathf.FloorToInt(timer / 60),
                                        Mathf.FloorToInt(timer % 60),
                                        Mathf.FloorToInt(timer % 1 * 1000));
        }
        else if (p2)
        {
            timer += 2;
            summaryMesh.text = "Solved! (+2)\n" +
                                string.Format("{0:00}:{1:00}.{2:000}",
                                        Mathf.FloorToInt(timer / 60),
                                        Mathf.FloorToInt(timer % 60),
                                        Mathf.FloorToInt(timer % 1 * 1000));
        }
        else
        {
            summaryMesh.text = "Solved!\n" +
                                string.Format("{0:00}:{1:00}.{2:000}",
                                        Mathf.FloorToInt(timer / 60),
                                        Mathf.FloorToInt(timer % 60),
                                        Mathf.FloorToInt(timer % 1 * 1000));
        }
    }

}
