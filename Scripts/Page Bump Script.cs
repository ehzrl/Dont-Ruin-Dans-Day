using System;
using System.Collections;
using UnityEngine;

public class PageBumpScript : MonoBehaviour
{
    private Boolean moved;
    private ArrayList paths;
    private GameObject page;
    Vector3 p;
    Quaternion a;
    float angle;
    public float movespeed = 10;
    public float rotateSpeed = 100;
    public Boolean rotateStarted;
    //public Boolean movementStarted;
    public Boolean ready;
    public int count;
    public Vector3 point;
    private float offBy;
    // private int currentIndex; // current index in GM path array
    //private Boolean indexUpdated; // if indx is updated, set to true. only update once


    // Start is called before the first frame update
    void Start()
    {
        page = GameObject.Find("Page at corner");

        page.SetActive(true);
        moved = false;

        // initalize arraylist
        paths = new ArrayList();

        paths.Add(new Vector3(-27f, 270f, -36.9f));
        paths.Add(new Vector3(-27f, 0f, -21.5f));


        a = Quaternion.AngleAxis((float)270, Vector3.up);
        p = new Vector3((float)20, (float)0.91, (float)-36.9); // starting position;

        page.transform.SetPositionAndRotation(p, a);
        rotateStarted = false;
        //movementStarted = false;
        ready = true;
        count = 0;
        point = new Vector3();
        offBy = 1f;

        Debug.Log("Starting Position (X, Z): (" + p.x + "," + p.z + ")");
        Debug.Log("Starting Angle Y is" + a.eulerAngles.y);

        page.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(" Wait for ui: " + GM.waitForUI);


        if (!(GM.waitToBump) && !moved)
        {
            page.SetActive(true);
            MovePage();
        }
    }

    public void MovePage()
    {

        Debug.Log("In move page");

        // move page
        // if covered all points, do nothing
        if (!(ready && !NextPointExists()))
        {
            // if ready = false transform values
            if (!ready)
            {
                Debug.Log("PAGE Current Path: Yrot: " + point.y + " X: " + point.x + " Z: " + point.z);

                // Booleans, if all are true, we're ready to go to the next point
                Boolean ycase = updateCase(point.y, 0);
                Boolean xcase = updateCase(point.x, 1);
                Boolean zcase = updateCase(point.z, 2);

                Debug.Log("PAGE Cases: Y: " + ycase + " X: " + xcase + " Z: " + zcase);

                // rotation vector may be the same or new value
                // if new value, set rotation started to true and start rotating
                if (!ycase)
                {
                    Debug.Log("PAGE Rotate started. Current y: " + page.transform.rotation.eulerAngles.y + " Needs to be " + point.y);

                    // if new value, set rotation started to true and start rotating
                    rotateStarted = true;

                    // Find quickest rotation:
                    float diff = page.transform.rotation.eulerAngles.y - point.y; // curr angle - new angle

                    Boolean flip = GetFlip(point.y, page.transform.rotation.eulerAngles.y);

                    // if flip is true
                    if (!flip && diff < 0 || flip && diff > 0)
                    {
                        Debug.Log("PAGE Rotate right:");
                        RotateRight();
                    } // call right fuction
                    else
                    {
                        Debug.Log("PAGE Rotate left:");
                        RotateLeft();
                    } // call left fuction


                    // update booleans
                    ycase = updateCase(point.y, 0);

                    Debug.Log("PAGE Updated Y: " + page.transform.rotation.eulerAngles.y + " Needs to be: " + point.y + " Case: " + ycase);

                    // finish rotating when angle is within range of new value
                    // set rotationStarted to false
                    if (ycase)
                    {
                        Debug.Log("PAGE Rotation completed. Y:" + page.transform.rotation.eulerAngles.y + " Is in range of " + point.y);
                        rotateStarted = false;
                    }
                }

                // if rotationStarted is true, don't move position
                if (!rotateStarted)
                {
                    Debug.Log("PAGE Rotation completed detected");
                    // will either need to move in x or z direction
                    if (!xcase)
                    {
                        Debug.Log("PAGEBNeed to move x. X: " + page.transform.position.x + " Want: " + point.x);
                        // move in x dir
                        // if x < point.x, move right
                        if (page.transform.position.x < point.x)
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
                        Debug.Log("PAGE Need to move z. Z: " + page.transform.position.z + " Want: " + point.z);
                        // move in z dir
                        // if z < point.z, move up
                        if (page.transform.position.z < point.z)
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
                    else { Debug.Log("PAGE X and Z cases passed, xcase = " + xcase + " zcase = " + zcase); }

                } // end of case move (x OR z)

                // if transform matches vector, go to next point
                if (xcase && ycase && zcase)
                {
                    Debug.Log("PAGE All cases passed, set ready to true");
                    ready = true;
                }
            }

            // else if ready = true, see if theres a next path
            else
            {
                if (NextPointExists())
                {
                    if (count == 1)
                    {
                        GM.bump = true; // set bump to true
                        Debug.Log("PAGE BUMPED");
                    }

                    // if next path, set path to next path
                    point = (Vector3)paths[count];
                    ready = false;
                    count++;
                    Debug.Log("PAGE Setting path " + (count - 1));
                }
            }

        } // end of if all points completed
        else
        {
            Debug.Log("PAGE All done with points");
            moved = true;
            page.SetActive(false);
            // increment paths counter for GM
            // if gm = current number, increment, else, don't
        }

         
    }

    // helper fuctions

    public Boolean updateCase(float a, int i) // a = point
    {
        Boolean madeCase = false;

        switch (i)
        {
            case 0: // y case
                {
                    float b = page.transform.rotation.eulerAngles.y;
                    //float bounds = offBy * 4;

                    // case for 0 and 360. If num approaches either, it counts
                    if (((b >= -offBy) && (b <= 2 * offBy)) || ((b >= 360 - (2 * offBy)) && (b <= 360)))
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

                        madeCase = ((b < a + offBy) && (b > a - offBy)) || ((v < a + offBy) && (v > a - offBy));
                    }
                    else // value not 0 or 360
                        madeCase = (b < a + offBy) && (b > a - offBy);
                    break;
                }
            case 1: // x case
                {
                    madeCase = (page.transform.position.x < a + offBy) && (page.transform.position.x > a - offBy);
                    break;
                }
            case 2: // z case
                {
                    madeCase = (page.transform.position.z < a + offBy) && (page.transform.position.z > a - offBy);
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
        if (c <= paths.Count)
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

    public void RotateLeft()
    {
        // move left
        float angle = -rotateSpeed * Time.deltaTime;

        a *= Quaternion.AngleAxis(angle, Vector3.up);

        page.transform.rotation = a;
        Debug.Log("PAGE Angle is now: " + a.eulerAngles.y);
    }

    public void RotateRight()
    {
        // move right
        float angle = rotateSpeed * Time.deltaTime;

        a *= Quaternion.AngleAxis(angle, Vector3.up);

        page.transform.rotation = a; // update angle
        Debug.Log("PAGE Angle is now: " + a.eulerAngles.y);
    }

    public void MoveForward()
    {
        // move up
        float position = movespeed * Time.deltaTime;

        p += Vector3.forward * position;
        page.transform.position = p; // update position
        Debug.Log("PAGE Position z is now: " + p.z);
    }

    public void MoveBack()
    {
        // move down
        float position = -movespeed * Time.deltaTime;

        p += Vector3.forward * position;
        page.transform.position = p; // update position
        Debug.Log("PAGE Position z is now: " + p.z);
    }

    public void MoveLeft()
    {
        // move left
        float position = -movespeed * Time.deltaTime;

        p += Vector3.right * position;
        page.transform.position = p; // update position
        Debug.Log("PAGE Position x is now: " + p.x);
    }

    public void MoveRight()
    {
        // move right
        float position = movespeed * Time.deltaTime;

        p += Vector3.right * position;
        page.transform.position = p; // update position
        Debug.Log("PAGE Position x is now: " + p.x);
    }

    public Boolean GetFlip(float a, float b)
    // a = point.y
    // b = transform
    {
        Debug.Log("PAGE DETERMINE FLIP FOR Curr: " + b + "New: " + a);

        float r = Math.Abs(a - b);
        float s = 360 - r;

        float m = Math.Min(s, r);

        // if min A
        if (m == r)
        {
            Debug.Log("PAGE flip false");
            return false;
        }
        // else if min B
        else
        {
            Debug.Log("PAGE flip true");
            return true;
        }

       
    } // end of get flip

}

