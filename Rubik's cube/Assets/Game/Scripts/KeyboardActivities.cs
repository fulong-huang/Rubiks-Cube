using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardActivities : MonoBehaviour
{
    private List<string> Q;

    private bool busy = false;
    private bool ctrl = false;
    private bool shift = false;

    private CubeRotate CubeRotate;

    private void Start()
    {
        Q = new List<string>();
        CubeRotate = GetComponent<CubeRotate>();
    }

    public bool BUSY()
    {
        return Q.Count != 0;
    }

    // Update is called once per frame
    public void KeyboardInputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))    ctrl = true;
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))        ctrl = false;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))        shift = true;
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))        shift = false;

        if (Input.GetKeyDown(KeyCode.U)) AddQ("U");
        if (Input.GetKeyDown(KeyCode.D)) AddQ("D");
        if (Input.GetKeyDown(KeyCode.R)) AddQ("R");
        if (Input.GetKeyDown(KeyCode.L)) AddQ("L");
        if (Input.GetKeyDown(KeyCode.F)) AddQ("F");
        if (Input.GetKeyDown(KeyCode.B)) AddQ("B");
        if (Input.GetKeyDown(KeyCode.M)) AddQ("M");
        if (Input.GetKeyDown(KeyCode.E)) AddQ("E");
        if (Input.GetKeyDown(KeyCode.S)) AddQ("S");

        if (Input.GetKeyDown(KeyCode.X)) AddQ("X");
        if (Input.GetKeyDown(KeyCode.Y)) AddQ("Y");
        if (Input.GetKeyDown(KeyCode.Z)) AddQ("Z");

        ProcessInput();
    }

    public bool ProcessInput()
    {
        if (!busy && Q.Count != 0)
        {
            busy = true;
            CompleteTask();
        }
        return busy;
    }

    public void AddQ(string s)
    {
        if (ctrl)
        {
            if(s != "M" && s != "S" && s != "E" && s != "X" && s != "Y" && s != "Z")
                s = s.ToLower();
        }
        int size = Q.Count - 1;
        if (size != -1)
        {
            string move = Q[size];
            if (move.Length == 1)
            {
                if (move == s)
                {
                    if (shift) Q.RemoveAt(size);
                    else Q[size] = s + "2";
                }
                else {
                    if (shift) s += "'";
                    Q.Add(s);
                }
            }
            else if (move.Substring(0,1) == s)
            {
                move = move.Substring(1, 1);
                if (move == "'")
                {
                    if (shift) Q[size] = s + "2";
                    else Q.RemoveAt(size);
                }
                else if (move == "2")
                {
                    if (shift) Q[size] = s;
                    else Q[size] = s + "'";
                }
            }
            else
            {
                if (shift) s += "'";
                Q.Add(s);
            }
        }
        else
        {
            if (shift)  s += "'";
            Q.Add(s);
        }
    }

    private void CompleteTask()
    {
        string s = Q[0];
        Q.RemoveAt(0);
        CubeRotate.CompleteTask(s);
    }

    public void FinishTask()
    {
        busy = false;
    }
}
