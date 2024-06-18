using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraScript : MonoBehaviour
{
    
    Vector3 p;
    Quaternion a;

    // Start is called before the first frame update
    void Start()
    {
        GM.InitGame();
        a = GM.getAng();
        p = GM.getPos();
        transform.SetPositionAndRotation(p, a);
        GM.sceneMoving = false;

        
    }

    // Update is called once per frame
    void Update()
    {
       // get to closest sidewalk

    }


}
