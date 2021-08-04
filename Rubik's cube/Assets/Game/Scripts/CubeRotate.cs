using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    public Transform target;
    public float speed = 500f;

    private CubeHolder cubeHolder;
    private KeyboardActivities keyboardActivities;

    private int[] currentSide;
    int piece;
    private bool busy = false;

    private void Start()
    {
        cubeHolder = GetComponent<CubeHolder>();
        keyboardActivities = GetComponent<KeyboardActivities>();
    }

    private void Update()
    {
        
        if (busy)
        {
            RotateSide();
        }
    }

    public void CompleteTask(string s)
    {
        int n = 90;
        if (s.Length != 1)
        {
            if (s.Substring(1, 1) == "'") n = -90;
            if (s.Substring(1, 1) == "2") n = 180;
            s = s.Substring(0, 1);
        }
        switch (s)
        {
            case "U":
                GroupAndRotateSide(Side.UP, n);
                break;
            case "D":
                GroupAndRotateSide(Side.DOWN, n);
                break;
            case "R":
                GroupAndRotateSide(Side.RIGHT, n);
                break;
            case "L":
                GroupAndRotateSide(Side.LEFT, n);
                break;
            case "F":
                GroupAndRotateSide(Side.FRONT, n);
                break;
            case "B":
                GroupAndRotateSide(Side.BACK, n);
                break;
            case "E":
                GroupAndRotateSide(Side.EQUATOR, n);
                break;
            case "M":
                GroupAndRotateSide(Side.MIDDLE, n);
                break;
            case "S":
                GroupAndRotateSide(Side.STANDING, n);
                break;
            case "X":
                GroupAndRotateSide(Side.X, n);
                break;
            case "Y":
                GroupAndRotateSide(Side.Y, n);
                break;
            case "Z":
                GroupAndRotateSide(Side.Z, n);
                break;
            case "u":
                GroupAndRotateSide(Side.WUP, n);
                break;
            case "d":
                GroupAndRotateSide(Side.WDOWN, n);
                break;
            case "r":
                GroupAndRotateSide(Side.WRIGHT, n);
                break;
            case "l":
                GroupAndRotateSide(Side.WLEFT, n);
                break;
            case "f":
                GroupAndRotateSide(Side.WFRONT, n);
                break;
            case "b":
                GroupAndRotateSide(Side.WBACK, n);
                break;
        }
        cubeHolder.SwitchPieces(currentSide, n);
    }

    private void GroupAndRotateSide(int[] Side, int n)
    {
        currentSide = Side;
        if (currentSide.Length == 9)
            piece = currentSide[0];
        else
        {
            piece = 26;
            cubeHolder.Pieces[piece].transform.localRotation = cubeHolder.Pieces[currentSide[0]].transform.localRotation;
        }
        target.localRotation = cubeHolder.Pieces[piece].transform.localRotation;
        cubeHolder.GroupSide(currentSide, piece);
        target.transform.Rotate(0, n, 0, Space.Self);
        busy = true;
    }

    private void RotateSide()
    {
        cubeHolder.Pieces[piece].transform.localRotation = 
            Quaternion.RotateTowards(cubeHolder.Pieces[piece].transform.localRotation, target.localRotation, speed * Time.deltaTime);

        if (Quaternion.Angle(cubeHolder.Pieces[piece].transform.localRotation, target.localRotation) <= 0.1)
        {
            cubeHolder.Pieces[piece].transform.localRotation = target.localRotation;
            cubeHolder.UnGroupSide(currentSide, piece);
            keyboardActivities.FinishTask();
            busy = false;
        }
    }
}
