using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivitiController : MonoBehaviour
{
    private KeyboardActivities keyboardActivities;
    private MouseActivities mouseActivities;
    private CubeRotate cubeRotate;

    private bool scrambling = false;
    private float initialSpeed;


    // Start is called before the first frame update
    void Start()
    {
        keyboardActivities = GetComponent<KeyboardActivities>();
        mouseActivities = GetComponent<MouseActivities>();
        cubeRotate = GetComponent<CubeRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrambling)
        {
            if (!keyboardActivities.ProcessInput())
            {
                scrambling = false;
                cubeRotate.speed = initialSpeed;
            }
        }
        else
        {
            if (!keyboardActivities.BUSY())
            {
                mouseActivities.MouseInputUpdate();
            }
            if (!mouseActivities.BUSY())
            {
                keyboardActivities.KeyboardInputUpdate();
            }
        }
    }

    public void Scramble(bool blind=false)
    {
        if (scrambling) return;
        scrambling = true;
        initialSpeed = cubeRotate.speed;
        cubeRotate.speed = initialSpeed * 3;
        int decision;
        int turns = 20;
        int bldTurns = Random.Range(0, 6);
        if (blind) {
            if (bldTurns != 0) {
                if(bldTurns < 3)
                {
                    bldTurns = 1;
                }
                else
                {
                    bldTurns = 2;
                }
            }
            turns += bldTurns;
        }

        int[] scrambles = new int[turns];

        scrambles[0] = Random.Range(0, 6);
        for (int i = 1; i < turns; i++)
        {
            decision = Random.Range(0, 6);
            scrambles[i] = decision;
            if (scrambles[i - 1] == decision)
                i--;
            else if(i > 1)
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
        for (int i = 0; i < 20; i++)
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
        if (blind)
        {
            for (int i = 0; i < bldTurns; i++)
            {
                switch (scrambles[19+i])
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

}
