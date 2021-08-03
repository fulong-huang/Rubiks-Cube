using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHolder : MonoBehaviour
{
/*
 *                       ________________________
 *                      /  6       7       8    /
 *                     /  3       4       5    /
 *                    /  0       1       2    /
 *                   /_______________________/
 *                      _________________________
 *                     /   14      15      16   /
 *                    /   12     |26|     13   /
 *                   /   9       10      11   /
 *                  /________________________/
 *                     __________________________
 *                    /    23     24      25    /
 *                   /    20     21      22    /
 *                  /    17     18      19    /
 *                 /_________________________/
 */

    public GameObject[] Pieces = new GameObject[27];


    public void GroupSide(int[] side, int piece)
    {
        for (int i = 1; i < side.Length; i++)
        {
            Pieces[side[i]].transform.parent = Pieces[piece].transform;
        }
    }
    public void UnGroupSide(int[] side, int piece)
    {
        for (int i = 1; i < side.Length; i++)
        {
            Pieces[side[i]].transform.parent = Pieces[piece].transform.parent;
        }
    }


    public void SwitchPieces(int[] side, int n)
    {
        if (n < 0) n += 360;
        n /= 90;
        GameObject obj1;
        GameObject obj2;
        if(side.Length <= 10){
            for(int _ = 0; _ < n; _++){
                obj1 = Pieces[side[8]];
                obj2 = Pieces[side[7]];
                for(int i = 8; i >= 3; i--){
                    Pieces[side[i]] = Pieces[side[i-2]];
                }
                Pieces[side[2]] = obj1;
                Pieces[side[1]] = obj2;
            }
        }
        else if(side.Length == 17){
            for(int _ = 0; _ < n; _++){
                obj1 = Pieces[side[8]];
                obj2 = Pieces[side[7]];
                for(int i = 8; i >= 3; i--){
                    Pieces[side[i]] = Pieces[side[i-2]];
                }
                Pieces[side[2]] = obj1;
                Pieces[side[1]] = obj2;

                
                obj1 = Pieces[side[16]];
                obj2 = Pieces[side[15]];
                for(int i = 16; i >= 11; i--){
                    Pieces[side[i]] = Pieces[side[i-2]];
                }
                Pieces[side[10]] = obj1;
                Pieces[side[9]] = obj2;
            }
        }
        else{
            for(int _ = 0; _<n; _++){
                for(int i = 8; i <= 30; i += 8){
                    obj1 = Pieces[side[i]];
                    obj2 = Pieces[side[i-1]];
                    for(int j = i; j >= i-5; j--){
                        Pieces[side[j]] = Pieces[side[j-2]];
                    }
                    Pieces[side[i-6]] = obj1;
                    Pieces[side[i-7]] = obj2;
                }
            }
        }
    }


}



public class Side
{
    public static int[] CENTERS =   { 4,  21, 10, 15, 12, 13 };

    public static int[] X =         { 13, 2, 5, 8, 16, 25, 22, 19, 11,
                                          1, 4, 7, 15, 24, 21, 18, 10,
                                          0, 3, 6, 14, 23, 20, 17, 9,
                                      13, 26, 12};
    public static int[] Y =         { 4, 2, 1, 0, 3, 6, 7, 8, 5,
                                         11, 10, 9, 12, 14, 15, 16, 13,
                                         19, 18, 17, 20, 23, 24, 25, 22,
                                      4, 26, 21};
    public static int[] Z =         { 10, 0, 1, 2, 11, 19, 18, 17, 9,
                                          3, 4, 5, 13, 22, 21, 20, 12,
                                          6, 7, 8, 16, 25, 24, 23, 14,
                                      10, 26, 15};
    public static int[] UP =        { 4, 0, 3, 6, 7, 8, 5, 2, 1 };
    public static int[] DOWN =      { 21, 17, 18, 19, 22, 25, 24, 23, 20 };
    public static int[] RIGHT =     { 13, 2, 5, 8, 16, 25, 22, 19, 11 };
    public static int[] LEFT =      { 12, 6, 3, 0, 9, 17, 20, 23, 14 };
    public static int[] FRONT =     { 10, 0, 1, 2, 11, 19, 18, 17, 9 };
    public static int[] BACK =      { 15, 8, 7, 6, 14, 23, 24, 25, 16 };
    public static int[] MIDDLE =    { 12, 1, 10, 18, 21, 24, 15, 7, 4 , 26};
    public static int[] EQUATOR =   { 21, 9, 10, 11, 13, 16, 15, 14, 12 , 26};
    public static int[] STANDING =  { 10, 3, 4, 5, 13, 22, 21, 20, 12 , 26};

    public static int[][] ALLSIDE = { UP, DOWN, RIGHT, LEFT, FRONT, BACK,
                                        MIDDLE, EQUATOR, STANDING};

    public static int[][] XYZ = {X, Y, Z};

    public static int[] WUP = { 4, 0, 3, 6, 7, 8, 5, 2, 1, 12, 14, 15, 16, 13, 11, 10, 9};
    public static int[] WDOWN = { 21, 17, 18, 19, 22, 25, 24, 23, 20, 9, 10, 11, 13, 16, 15, 14, 12 };
    public static int[] WRIGHT = { 13, 2, 5, 8, 16, 25, 22, 19, 11, 4, 7, 15, 24, 21, 18, 10, 1};
    public static int[] WLEFT = { 12, 6, 3, 0, 9, 17, 20, 23, 14, 1, 10, 18, 21, 24, 15, 7, 4 };
    public static int[] WFRONT = { 10, 0, 1, 2, 11, 19, 18, 17, 9, 3, 4, 5, 13, 22, 21, 20, 12 };
    public static int[] WBACK = { 15, 8, 7, 6, 14, 23, 24, 25, 16, 12, 20, 21, 22, 13, 5, 4, 3 };


}