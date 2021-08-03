
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseActivities : MonoBehaviour
{
    public int speed = 500;
    public int Sensitive = 1000;
    private bool leftClick  = false;
    private bool rightClick = false;

    private bool moving = false;
    private bool resetting = false;

    private RaycastHit hit;
    private int[] rotatingSide;
    private int rotatingPiece;
    private CubeHolder cubeHolder;

    private Vector2 Direction;
    private Vector2 MouseAxis;

    private float RotatedAngle;
    private Vector3 TargetRotation;

    private void Start(){
        cubeHolder = GetComponent<CubeHolder>();
    }

    // Update is called once per frame
    public bool BUSY(){
        return resetting || moving;
    }
    public void MouseInputUpdate()
    {
        if(!resetting){ 
            if (Input.GetMouseButtonDown(0)) // Left Click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)){
                    leftClick = true;
                }
            }
            else if(Input.GetMouseButtonDown(1)){
                rightClick = true;
            }
        }
        else{
            RoundRotation();
        }


        if(leftClick){
            LeftClickDrag();
            if (Input.GetMouseButtonUp(0))
            {
                if(moving){
                    TargetRotation = cubeHolder.Pieces[rotatingPiece].transform.localEulerAngles;
                    TargetRotation = RoundNearest(TargetRotation);
                    moving = false;
                    resetting = true;
                }
                leftClick = false;
            }
        }
        if(rightClick){
            // If (NOT freelook)
            { 
                RightClickDrag();
            }

            if(Input.GetMouseButtonUp(1)){
                if(moving){
                    TargetRotation = cubeHolder.Pieces[rotatingPiece].transform.localEulerAngles;
                    TargetRotation = RoundNearest(TargetRotation);
                    moving = false;
                    resetting = true;
                }
                rightClick = false;
            }
        }
    }

    private void RightClickDrag(){
        GetMouseAxis();
        if(!moving){
            Decision(Side.XYZ);
        }
        else{
            float num = -(MouseAxis.x*Direction.x + MouseAxis.y*Direction.y);
            num *= Sensitive*Time.deltaTime;
            RotatedAngle += num;
            cubeHolder.Pieces[rotatingPiece].transform.Rotate(0, num, 0);
        }
    }
    private void LeftClickDrag(){
        GetMouseAxis();
        if(!moving){
            Decision(Side.ALLSIDE);
        }
        else{
            float num = -(MouseAxis.x*Direction.x + MouseAxis.y*Direction.y);
            num *= Sensitive*Time.deltaTime;
            RotatedAngle += num;
            cubeHolder.Pieces[rotatingPiece].transform.Rotate(0, num, 0);
        }
    }
    private void GetMouseAxis(){
        MouseAxis.x = Input.GetAxis("Mouse X");
        MouseAxis.y = Input.GetAxis("Mouse Y");
    }

    private void Decision(int[][] targetMove){
        if(MouseAxis.magnitude <= 0.1) return;

        RotatedAngle = 0;

        moving = true;
        Vector3 v0, v1;
        Vector2 v2, v3;
        if(targetMove.Length == 3){
            v0 = cubeHolder.Pieces[12].transform.up;
            v1 = cubeHolder.Pieces[21].transform.up;
            MouseAxis.Normalize();
            v2 = new Vector2(v0.z, v0.y);
            v3 = new Vector2(v1.z, v1.y);
            v2.Normalize();
            v3.Normalize();
            
            float a = Mathf.Min(Vector2.Distance( MouseAxis, v2),
                                Vector2.Distance(-MouseAxis, v2));
            float b = Mathf.Min(Vector2.Distance( MouseAxis, v3),
                                Vector2.Distance(-MouseAxis, v3));
                                
            if(a > b){
                rotatingSide = targetMove[0];
                Direction.x = -v2.y;
                Direction.y =  v2.x;
            }
            else{
                rotatingSide = targetMove[1];
                Direction.x = -v3.y;
                Direction.y =  v3.x;
                a = b;
            }
            v0 = cubeHolder.Pieces[10].transform.up;
            v2 = new Vector2(v0.z, v0.y);
            v2.Normalize();

            b = Mathf.Min(Vector2.Distance( MouseAxis, v2),
                          Vector2.Distance(-MouseAxis, v2));
            if(a < b){
                rotatingSide = targetMove[2];
                Direction.x =  v2.y;
                Direction.y = -v2.x;
            }
        }
        else{
            List<int[]> foundSides = new List<int[]>();
            foreach(int[] s in targetMove){
                for(int i = 1; i < 9; i++){
                    if(cubeHolder.Pieces[s[i]].transform.name == hit.transform.name){
                        if(cubeHolder.Pieces[s[0]].transform.up != hit.normal)
                        {
                            foundSides.Add(s);
                        }
                        break;
                    }
                }
            }
            v0 = Vector3.Cross(hit.normal, cubeHolder.Pieces[foundSides[0][0]].transform.up);
            v1 = Vector3.Cross(hit.normal, cubeHolder.Pieces[foundSides[1][0]].transform.up);
            MouseAxis.Normalize();
            v2 = new Vector2(v0.z, v0.y);
            v3 = new Vector2(v1.z, v1.y);
            v2.Normalize();
            v3.Normalize();

            float a = Mathf.Min(Vector2.Distance( MouseAxis, v2),
                                Vector2.Distance(-MouseAxis, v2));
            float b = Mathf.Min(Vector2.Distance( MouseAxis, v3),
                                Vector2.Distance(-MouseAxis, v3));

            if(a<b){
                rotatingSide = foundSides[0];
                Direction = v2;
            }
            else{
                rotatingSide = foundSides[1];
                Direction = v3;
            }
        }

        if(rotatingSide.Length >= 10){
            rotatingPiece = 26;
            cubeHolder.Pieces[26].transform.localEulerAngles = 
                    cubeHolder.Pieces[rotatingSide[0]].transform.localEulerAngles;
        }
        else{
            rotatingPiece = rotatingSide[0];
        }
        cubeHolder.GroupSide(rotatingSide, rotatingPiece);

    }

    private void RoundRotation(){
        Quaternion dir = Quaternion.RotateTowards(
            cubeHolder.Pieces[rotatingPiece].transform.localRotation,
            Quaternion.Euler(TargetRotation),
            speed*Time.deltaTime
        );
        cubeHolder.Pieces[rotatingPiece].transform.localRotation = dir;
        if(Quaternion.Angle(cubeHolder.Pieces[rotatingPiece].transform.localRotation,
                  Quaternion.Euler(TargetRotation)) <= 0.1){
            cubeHolder.Pieces[rotatingPiece].transform.localRotation = Quaternion.Euler(TargetRotation);
            
            cubeHolder.UnGroupSide(rotatingSide, rotatingPiece);
            
            RotatedAngle %= 360;
            if(RotatedAngle < 0)
                RotatedAngle += 360;
            if(RotatedAngle % 90 > 45)
                RotatedAngle = (int)RotatedAngle/90 *90+90;
            else
                RotatedAngle = (int)RotatedAngle/90 *90;
            cubeHolder.SwitchPieces(rotatingSide, (int)RotatedAngle);
            resetting = false;
        }
    }

    private Vector3 RoundNearest(Vector3 v){
        int num;
        for(int i = 0; i < 3; i++){
            if(v[i] < 0){
                v[i] += 360;
            }
            num = (int)v[i]/90;
            v[i] %= 90;
            if(v[i] >= 45){
                v[i] = (num+1)*90;
            }
            else{
                v[i] = num*90;
            }
        }

        return v;
    }
}
