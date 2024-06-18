using System;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
   
    Vector3 p;
    Quaternion a;
    float angle;
    public float movespeed = 10;
    public float rotateSpeed = 40;
    public Boolean rotateStarted;
    //public Boolean movementStarted;
    public Boolean ready;
    public int count;
    public Vector3 point;
    private float offBy;
    private int currentIndex; // current index in GM path array
    private Boolean indexUpdated; // if indx is updated, set to true. only update once
    private Boolean igiveup;

    // Start is called before the first frame update
    void Start()
    {

        // put camera at start position
        currentIndex = GM.getIndex();
        if(currentIndex == 10 || currentIndex == 11)
        {
            Vector3 e = (Vector3)GM.paths[currentIndex][0];
            // update game manager
            GM.setPos(new Vector3(e.x, (float)1.77, (float)e.z));
            GM.setAng(Quaternion.AngleAxis((float)e.y, Vector3.up));
            // a = Quaternion.AngleAxis((float)e.y, Vector3.up); // camera starting angle
            //p = new Vector3(e.x, (float)1.77, (float)e.z);
        }
       
        if(currentIndex == 2)
        {
            igiveup = true;
        }
        else
        {
            igiveup = false;
        }
     
        a = GM.getAng();
        p = GM.getPos();
      

        transform.SetPositionAndRotation(p, a);
        rotateStarted = false;
        //movementStarted = false;
        ready = true;
        count = 0;
        point = new Vector3();
        offBy = 1f;
        indexUpdated = false;

        Debug.Log("CURRENT INDEX: " + currentIndex + "SceneSTART = " + GM.sceneStart);


        Debug.Log("Starting Position (X, Z): (" + p.x + "," + p.z + ")");
        Debug.Log("Starting Angle Y is" + a.eulerAngles.y);

        GM.sceneMoving = false;
        GM.sceneDone = false;
    }



    // Update is called once per frame
    void Update()
    {
        Debug.Log("CAMERA scene moving = " + GM.sceneMoving);

        if (!GM.waitForUI)
        {
            if( currentIndex == 2)
            {
                GM.sceneStart = true;
            }
            

            // if covered all points, do nothing
            if (!(ready && !NextPointExists()))
            {
                GM.sceneMoving = true;

                // if ready = false transform values
                if (!ready)
                {
                    Debug.Log("Current Path: Yrot: " + point.y + " X: " + point.x + " Z: " + point.z);

                    // Booleans, if all are true, we're ready to go to the next point
                    Boolean ycase = updateCase(point.y, 0);
                    Boolean xcase = updateCase(point.x, 1);
                    Boolean zcase = updateCase(point.z, 2);

                    Debug.Log("Cases: Y: " + ycase + " X: " + xcase + " Z: " + zcase);

                    // rotation vector may be the same or new value
                    // if new value, set rotation started to true and start rotating
                    if (!ycase)
                    {
                        Debug.Log("Rotate started. Current y: " + transform.rotation.eulerAngles.y + " Needs to be " + point.y);

                        // if new value, set rotation started to true and start rotating
                        rotateStarted = true;

                        // Find quickest rotation:
                        float diff = transform.rotation.eulerAngles.y - point.y; // curr angle - new angle

                        Boolean flip = GetFlip(point.y, transform.rotation.eulerAngles.y);

                        // if flip is true
                        if (!flip && diff < 0 || flip && diff > 0)
                        {
                            Debug.Log("Rotate right:");
                            RotateRight();
                        } // call right fuction
                        else
                        {
                            Debug.Log("Rotate left:");
                            RotateLeft();
                        } // call left fuction


                        // update booleans
                        ycase = updateCase(point.y, 0);

                        Debug.Log("Updated Y: " + transform.rotation.eulerAngles.y + " Needs to be: " + point.y + " Case: " + ycase);

                        // finish rotating when angle is within range of new value
                        // set rotationStarted to false
                        if (ycase)
                        {
                            Debug.Log("Rotation completed. Y:" + transform.rotation.eulerAngles.y + " Is in range of " + point.y);
                            rotateStarted = false;
                        }
                    }

                    // if rotationStarted is true, don't move position
                    if (!rotateStarted)
                    {
                        Debug.Log("Rotation completed detected");
                        // will either need to move in x or z direction
                        if (!xcase)
                        {
                            Debug.Log("Need to move x. X: " + transform.position.x + " Want: " + point.x);
                            // move in x dir
                            // if x < point.x, move right
                            if (transform.position.x < point.x)
                            {
                                // move right function
                                MoveRight();
                            }
                            else // else, move left
                            {
                                // move left function
                                MoveLeft();
                            }

                            // update boolean
                            xcase = updateCase(point.x, 1);

                        }
                        else if (!zcase)
                        {
                            Debug.Log("Need to move z. Z: " + transform.position.z + " Want: " + point.z);
                            // move in z dir
                            // if z < point.z, move up
                            if (transform.position.z < point.z)
                            {
                                // move forward function
                                MoveForward();
                            }
                            else  // else, move backward
                            {
                                // move backward function
                                MoveBack();
                            }

                            // update boolean
                            zcase = updateCase(point.z, 2);
                        }
                        else { Debug.Log("X and Z cases passed, xcase = " + xcase + " zcase = " + zcase); }

                    } // end of case move (x OR z)

                    // if transform matches vector, go to next point
                    if (xcase && ycase && zcase)
                    {
                        Debug.Log("All cases passed, set ready to true");
                        ready = true;
                    }
                }

                // else if ready = true, see if theres a next path
                else
                {
                    if (NextPointExists())
                    {
                        // if next path, set path to next path
                        point = (Vector3)GM.paths[currentIndex][count];
                        ready = false;
                        count++; // increment count
                        Debug.Log("Setting path " + (count - 1));
                    }
                }

            } // end of if all points completed
            else
            {
                Debug.Log("All done with points");
                // increment paths counter for GM
                // if gm = current number, increment, else, don't
                if (!indexUpdated)
                {
                    GM.IncrIndex();
                    indexUpdated = true;
                    GM.sceneMoving = false;
                    
                }

                GM.sceneDone = true;

            }
        } // end of wait for UI
        else
        {
            GM.sceneMoving = false;
        }
        
    } // end of update

    public Boolean getIndexUpdated()
    {
        return indexUpdated;
    }

    public Boolean updateCase(float a, int i) // a = point
    {
        Boolean madeCase = false;

        switch (i)
        {
            case 0: // y case
                {
                    float b = transform.rotation.eulerAngles.y;
                    //float bounds = offBy * 4;

                    // case for 0 and 360. If num approaches either, it counts
                    if ( ( (b >= -offBy) && (b <= 2 * offBy) ) || ( (b >= 360 - (2 * offBy)) && (b <= 360) ) )
                    {
                        float v;
                        // check if 360 or zero 
                        if (b >= -offBy && b <= 2 * offBy)
                        {
                            v = 360; // if zero, set v to 360
                        }
                        else
                        {
                            v = 0; // if 360, set v to 0
                        }

                        madeCase = ( (b < a + offBy) && (b > a - offBy) ) || ( (v < a + offBy) && (v > a - offBy) );
                    } 
                    else // value not 0 or 360
                        madeCase = (b < a + offBy) && (b > a - offBy);
                    break;
                }
            case 1: // x case
                {
                    madeCase = (transform.position.x < a + offBy) && (transform.position.x > a - offBy);
                    break;
                }
            case 2: // z case
                {
                    madeCase = (transform.position.z < a + offBy) && (transform.position.z > a - offBy);
                    break;
                }
        }

        return madeCase;
    }
    
    public Boolean NextPointExists() // returns true if next point exists in array list
    {
        // increment counter
        int c = count + 1;
        // if count <= size of array, return true
        if(c <= GM.paths[currentIndex].Count)
        {
            return true;
        }
        // else return false
        else
        {
            return false;
        }
    }

    public Boolean OnSameSide(float a, float b)
    {
        int aVal;
        int bVal;

        if (a >= 0 && a <= 180)
        {
            aVal = 1;
        }
        else
        {
            aVal = 0;
        }
        if (b >= 0 && b <= 180)
        {
            bVal = 1;
        }
        else
        {
            bVal = 0;
        }

        if (aVal != bVal) { return false; }
        else { return true; }
    }
    // helper fuction for OneIsPolar, returns true if value is polar
    public Boolean IsPolar(float a)
    {
        if (180 - offBy < a && a < 180 + offBy)
        {
            return true;
        }
        else if (0 - offBy <= a && a <= 0 + offBy) // might have error
        {
            return true;
        }
        else if (360 - offBy <= a && a <= 360 + offBy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // If only one value is polar, return it. Else, return -1
    public float OneIsPolar(float a, float b)
    {
        if(IsPolar(a) && !IsPolar(b))
        {
            return a;
        }
        else if(!IsPolar(a) && IsPolar(b))
        {
            return b;
        }
        else { return -1; }
    }
    public void RotateLeft()
    {
            // move left
            float angle = -rotateSpeed * Time.deltaTime;
            GM.AngShift(angle);
            a = GM.getAng();
            transform.rotation = a;
            Debug.Log("Angle is now: " + a.eulerAngles.y);
    }

    public void RotateRight()
    {
            // move right
            float angle = rotateSpeed * Time.deltaTime;
            GM.AngShift(angle);
            a = GM.getAng();    // update a
            transform.rotation = a; // update angle
            Debug.Log("Angle is now: " + a.eulerAngles.y);
    }

    public void MoveForward()
    {
        // move up
        float position = movespeed * Time.deltaTime;
        GM.VertShift(position);
        p.z = GM.getPos().z; // update p
        transform.position = p; // update position
        Debug.Log("Position z is now: " + p.z);
    }

    public void MoveBack() 
    {
        // move down
        float position = -movespeed * Time.deltaTime;
        GM.VertShift(position);
        p.z = GM.getPos().z; // update p
        transform.position = p; // update position
        Debug.Log("Position z is now: " + p.z);
    }

    public void MoveLeft() 
    {
        // move left
        float position = -movespeed * Time.deltaTime;
        GM.HorizShift(position);
        p.x = GM.getPos().x; // update p
        transform.position = p; // update position
        Debug.Log("Position x is now: " + p.x);
    }

    public void MoveRight()
    {
        // move right
        float position = movespeed * Time.deltaTime;
        GM.HorizShift(position);
        p.x = GM.getPos().x; // update p
        transform.position = p; // update position
        Debug.Log("Position x is now: " + p.x);
    }

    public Boolean GetFlip(float a, float b)
    // a = point.y
    // b = transform
    {
        Debug.Log("DETERMINE FLIP FOR Curr: " + b + "New: " + a);

        float r = Math.Abs(a - b);
        float s = 360 - r;

        float m = Math.Min(s, r);

        // if min A
        if(m == r)
        {
            Debug.Log("flip false");
            return false;
        }
        // else if min B
        else
        {
            Debug.Log("flip true");
            return true;
        }

        /*
        Debug.Log("DETERMINE FLIP FOR Curr: " + b + "New: " + a);

        float polarVal = OneIsPolar(a, b);

        // if one value is polar
        if (polarVal != -1)
        {
            Debug.Log("One value is polar: " + polarVal);

            float nonPolarVal;

            // initalize value. if not one, its the other 
            if (polarVal >= a - offBy && polarVal <= a + offBy)
            { nonPolarVal = b; }
            else
            { nonPolarVal = a; }

            Debug.Log("Non polar value Is: " + nonPolarVal);

            // if polar side is 180
            if (polarVal >= 180 - offBy && polarVal <= 180 + offBy)
            {
                Debug.Log("Polar side " + polarVal + " == 180. Flip is true");
                // flip is true
                return true;
            }
            // if polar side is 0
            else if (polarVal >= 0 - offBy && polarVal <= 0 + offBy)
            {
                Debug.Log("Polar side " + polarVal + " == 0");

                // if nonpolar btwn 0-180
                if (nonPolarVal > 0 && nonPolarVal < 180)
                {
                    Debug.Log("Non polar side " + nonPolarVal + " is between 0 and 180. Flip is true");
                    return true; 
                }  // flip is true

                // if nonpolar btwn 180 - 360
                else
                {
                    Debug.Log("Non polar side " + nonPolarVal + " is between 180 and 360. Flip is false");
                    return false; 
                }  // flip is false
            }
            // if polar side is 360
            else
            {
                Debug.Log("Polar side " + polarVal + " == 360");

                // if non polar btwn 0 - 180
                if (nonPolarVal > 0 && nonPolarVal < 180)
                {
                    Debug.Log("Non polar side " + nonPolarVal + " is between 0 and 180. Flip is false");
                    return false; 
                }  // flip is false
                // if non polar btwn 180 - 360
                else
                {
                    Debug.Log("Non polar side " + nonPolarVal + " is between 180 and 360. Flip is true");
                    return true; 
                } // flip is true
            }

        }
        // else if degrees on same side
        else if (OnSameSide(a, b))
        {
            Debug.Log("On same side true for " + a + " and " + b + ", flip true");
            return true; // flip is true
        }
        // else (values are either both polar or not polar not on same side)
        else
        {
            Debug.Log("Both polar or not on same side true for " + a + " and " + b + ", flip false");
            return false; // flip is false
        }
        */
    } // end of get flip

}
