using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TerrainTools;
using UnityEngine.UIElements;



public static class GM
{
    private static Vector3 pos;
    private static Quaternion ang;
    public static ArrayList[] paths;
    public static int index;
    public static Boolean sceneMoving;
    public static Boolean sceneDone;
    public static Boolean bump;
    public static Boolean waitForUI;
    public static Boolean sceneStart;
    public static Boolean waitToBump;

   // private static ArrayList<float>[] paths;


    public static void InitGame()
    {
        // starting postiton
        pos = new Vector3(17, (float)1.77, (float)-37); // starting position
        ang = Quaternion.AngleAxis((float)40, Vector3.up); // camera starting angle
        paths = new ArrayList[12];
        sceneMoving = false;
        bump = false;
        waitForUI = false;
        sceneDone = false;
        sceneStart = false;
        waitToBump = true;
        index = 0; // intialize index
        
        // initalize array lists
        for(int i= 0; i < 12; i++) 
        {
            paths[i] = new ArrayList();
        }

        // S4
        paths[0].Add(new Vector3(17f, 5.322f, -34f));
        // S6
        paths[1].Add(new Vector3(17f, 180f, -38f));
        paths[1].Add(new Vector3(-19f, 270f, -38f));
        // S8
        paths[2].Add(new Vector3(-27f, 270f, -38f));
        paths[2].Add(new Vector3(-27f, 0f, -6f));
        paths[2].Add(new Vector3(-27f, 270f, -6f));
        // S10
        paths[3].Add(new Vector3(-41f, 270f, -6f));
        paths[3].Add(new Vector3(-41f, 180f, -21f));
        paths[3].Add(new Vector3(-41f, 270f, -21f));
        // S12
        paths[4].Add(new Vector3(-58f, 270f, -21f));
        // S14
        paths[5].Add(new Vector3(-58f, 0f, -8f));
        paths[5].Add(new Vector3(-68f, 270f, -8f));
        paths[5].Add(new Vector3(-68f, 0f, 22.4f));
        paths[5].Add(new Vector3(-50f, 90f, 22.4f));
        paths[5].Add(new Vector3(-50f, 0f, 28.2f));
        // S16
        paths[6].Add(new Vector3(-41f, 90f, 28.2f));
        // S18
        paths[7].Add(new Vector3(-27f, 90f, 28.2f));
        paths[7].Add(new Vector3(-27f, 0f, 32.3f));
        paths[7].Add(new Vector3(-27f, 90f, 32.3f));
        // S21
        paths[8].Add(new Vector3(-27f, 0f, 38f));
        paths[8].Add(new Vector3(52f, 90f, 38f));
        paths[8].Add(new Vector3(52f, 180f, 20f));
        // S23
        paths[9].Add(new Vector3(52f, 180f, -2f));
        paths[9].Add(new Vector3(28.4f, 270f, -2f));
        paths[9].Add(new Vector3(28.4f, 180f, -52f));
        paths[9].Add(new Vector3(-61f, 270f, -52f));
        paths[9].Add(new Vector3(-61f, 180f, -64.4f));
        // S24
        paths[10].Add(new Vector3(28.7f, 90, 14.6f));
        paths[10].Add(new Vector3(46.2f, 90, 14.6f));
        // S25
        paths[11].Add(new Vector3(-41.9f, 0f, 16.8f));
        paths[11].Add(new Vector3(-41.9f, 0f, 47.7f));
        paths[11].Add(new Vector3(-46.2f, 270f, 47.7f));




        // debug
        Debug.Log("Starting Position (X, Z): (" + pos.x + "," + pos.z + ")");
        Debug.Log("Starting Angle Y is" + ang.eulerAngles.y);
    }

    public static void IncrIndex()
    {
        index++;
    }

    public static int getIndex()
    {
        return index;
    }

    public static Vector3 getPos()
    {
        return pos;
    }

    public static Quaternion getAng() 
    {
        return ang;
    }

    public static void setPos(Vector3 p)
    {
        pos = p;
    }

    public static void setAng(Quaternion a) 
    {
        ang = a;
    }

    public static void VertShift(float a)
    {
        // pos.z += a;
        pos += Vector3.forward * a;
    }

    public static void HorizShift(float a)
    {
        // pos.x += a;
        pos += Vector3.right * a;
    }


    public static void AngShift(float a)
    {
        ang *= Quaternion.AngleAxis(a, Vector3.up);
    }


}
