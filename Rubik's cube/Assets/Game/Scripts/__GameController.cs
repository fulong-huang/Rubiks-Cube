using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class __GameController : MonoBehaviour
{
    // 0 -> Free Play,  1 -> Normal,  2 -> Blind;
    public int mode = 0;
    public Transform canv;
    public bool waiting = false;

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
                if (mode != 0)
                {
                    canv.GetChild(0).GetChild(3).gameObject.SetActive(true);
                    cubeHolder.StartNewState();
                    started = false;
                }
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
            if (!keyboardActivities.BUSY())
            {
                mouseActivities.MouseInputUpdate();
            }
            if (!mouseActivities.BUSY())
            {
                keyboardActivities.KeyboardInputUpdate();
            }
            if (mode == 1 && cubeHolder.CheckSolved())
            {
                waiting = true;
                uIController.solved();
                canv.GetChild(0).GetChild(5).gameObject.SetActive(true);
            }
        }

    }

    public void Scramble()
    {
        if (scrambling) return;
        scrambling = true;

        if (mode != 0)
        {
            HideSticker();
            canv.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }


        initialSpeed = cubeRotate.speed;
        cubeRotate.speed = initialSpeed * 3;
        int decision;
        int turns = 2;
        int bldTurns = Random.Range(0, 6);

        int[] scrambles;
        if (mode == 2)
        {
            if (bldTurns != 0)
            {
                if (bldTurns < 3)
                {
                    bldTurns = 1;
                }
                else
                {
                    bldTurns = 2;
                }
            }
            scrambles = new int[turns + bldTurns];
        }
        scrambles = new int[turns];

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
        if (mode == 2)
        {
            for (int i = 0; i < bldTurns; i++)
            {
                switch (scrambles[19 + i])
                {
                    case 0:
                        move = "r";
                        break;
                    case 1:
                        move = "l";
                        break;
                    case 2:
                        move = "f";
                        break;
                    case 3:
                        move = "b";
                        break;
                    case 4:
                        move = "u";
                        break;
                    case 5:
                        move = "d";
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
    }

    private void HideSticker()
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(Spelling.STICKER));
    }

    public void RevealStickers()
    {
        canv.GetChild(0).GetChild(3).gameObject.SetActive(false);
        Camera.main.cullingMask ^= 1 << LayerMask.NameToLayer(Spelling.STICKER);
        if (mode == 1)
        {
            canv.GetChild(2).gameObject.SetActive(true);
            canv.GetChild(0).GetChild(4).gameObject.SetActive(true);
            uIController.StartNormalGame();
            waiting = false;
        }
    }

    public void RestartGame()
    {
        canv.GetChild(1).gameObject.SetActive(false);
        canv.GetChild(0).GetChild(5).gameObject.SetActive(false);
        Scramble();
    }

}
