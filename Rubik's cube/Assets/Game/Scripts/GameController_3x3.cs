using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_3x3 : MonoBehaviour
{
    public Transform canv;
    private bool waiting = true;

    [SerializeField]
    private int turns = 20;
    private bool moved = false;

    private UIController uIController;

    private KeyboardActivities keyboardActivities;
    private MouseActivities mouseActivities;
    private CubeRotate cubeRotate;

    private CubeHolder cubeHolder;

    private bool scrambling = false;
    private float initialSpeed;
    private bool started;


    // Start is called before the first frame update
    void Start()
    {
        keyboardActivities = GetComponent<KeyboardActivities>();
        mouseActivities = GetComponent<MouseActivities>();
        cubeRotate = GetComponent<CubeRotate>();

        uIController = canv.GetComponent<UIController>();
        cubeHolder = GetComponent<CubeHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrambling)
        {
            if (!keyboardActivities.ProcessingInput())
            {
                scrambling = false;
                cubeRotate.speed = initialSpeed;

                canv.GetChild(0).GetChild(3).gameObject.SetActive(true);
                cubeHolder.StartNewState();
                started = false;
              
            }
        }
        else if (!waiting)
        {
            if (!started)
            {
                if (cubeHolder.GetStateChanged() == true)
                {
                    uIController.StartTimer();
                    started = true;
                }
            }
            if(started && moved && !keyboardActivities.BUSY() && !mouseActivities.BUSY())
            {
                if (cubeHolder.CheckSolved())
                {
                    waiting = true;
                    uIController.stopTimer();
                    uIController.solved();
                    canv.GetChild(0).GetChild(5).gameObject.SetActive(true);
                }
                
                moved = false;
            }
            if (!keyboardActivities.BUSY())
            {
                mouseActivities.MouseInputUpdate();
            }
            if (!mouseActivities.BUSY())
            {
                keyboardActivities.KeyboardInputUpdate();
            }
            if(keyboardActivities.BUSY() || mouseActivities.BUSY())
            {
                moved = true;
            }
        }

    }

    public void Scramble()
    {
        if (scrambling) return;
        scrambling = true;

        canv.GetChild(0).GetChild(2).gameObject.SetActive(false);

        HideSticker();

        initialSpeed = cubeRotate.speed;
        cubeRotate.speed = initialSpeed * 3;
        int decision;

        int[] scrambles = new int[turns];

        scrambles[0] = Random.Range(0, 6);
        for (int i = 1; i < turns; i++)
        {
            decision = Random.Range(0, 6);
            scrambles[i] = decision;
            if (scrambles[i - 1] == decision)
                i--;
            else if (i > 1)
            {
                if (decision % 2 == 0)
                {
                    if (scrambles[i - 1] == decision + 1)
                        if (scrambles[i - 2] == decision)
                        {
                            i--;
                        }
                }
                else if (scrambles[i - 1] == decision - 1)
                {
                    if (scrambles[i - 2] == decision)
                    {
                        i--;
                    }
                }
            }
        }

        string move = "M";
        for (int i = 0; i < turns; i++)
        {
            switch (scrambles[i])
            {
                case 0:
                    move = "R";
                    break;
                case 1:
                    move = "L";
                    break;
                case 2:
                    move = "F";
                    break;
                case 3:
                    move = "B";
                    break;
                case 4:
                    move = "U";
                    break;
                case 5:
                    move = "D";
                    break;
            }
            switch (Random.Range(0, 3))
            {
                case 0:
                    move += "'";
                    break;
                case 2:
                    move += "2";
                    break;
            }
            keyboardActivities.AddQ(move);
        }
    }

    private void HideSticker()
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(Spelling.STICKER));
    }

    private void RevealStickers()
    {
        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer(Spelling.STICKER);
    }

    public void StartTimer()
    {
        RevealStickers();

        canv.GetChild(0).GetChild(3).gameObject.SetActive(false);
        canv.GetChild(3).gameObject.SetActive(true);
        canv.GetChild(0).GetChild(4).gameObject.SetActive(true);
        uIController.StartNormalGame();
        waiting = false;
    }


    public void RestartGame()
    {
        canv.GetChild(1).gameObject.SetActive(false);
        canv.GetChild(0).GetChild(5).gameObject.SetActive(false);
        canv.GetChild(2).gameObject.SetActive(false);
        Scramble();
    }

}
