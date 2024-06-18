using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class S6UIScript : MonoBehaviour
{
    public GameObject buttons;
    public GameObject narration;
    public GameObject dialouge;
    public Text[] N;
    public Image[] D;
    public Boolean nActive;
    public Boolean dActive;
    public int nCounter;
    public int dCounter;
    public int waitTime = 5; // wait to change dialouge

    // Start is called before the first frame update
    void Start()
    {
        // Initalize Objects
        buttons = GameObject.Find("Buttons");
        narration = GameObject.Find("N1");
        dialouge = GameObject.Find("Dialouge");

        // set all to not visable
        narration.SetActive(true);
        dialouge.SetActive(true);
        buttons.SetActive(false);

        // initalize arrays
        N = narration.GetComponentsInChildren<Text>();
        D = dialouge.GetComponentsInChildren<Image>();

        // initalize bools
        nActive = false;
        dActive = false;

        // initalize counters
        nCounter = 0;
        dCounter = 0;

        Debug.Log("UI MANAGER, here are all the names of the narration objects");
        // set all components to false
        for (int i = 0; i < N.Length; i++)
        {
            Debug.Log(N[i].gameObject.name);
            N[i].gameObject.SetActive(false);
        }

        Debug.Log("UI MANAGER, here are all the names of the dialouge objects");
        // set all components to false
        for (int i = 0; i < D.Length; i++)
        {
            Debug.Log(D[i].gameObject.name);
            D[i].gameObject.SetActive(false);
        }



    }

    // Update is called once per frame
    void Update()
    {
        // if ncount < size
        if (nCounter < N.Length)
        {
            Debug.Log("S6 UI MANAGER: Narration called. Length:" + N.Length);
            // Narrate
            Narrarate();
        }

        // if dcount < size 
        if (dCounter < D.Length)
        {
            StartCoroutine(Speak());
            GM.waitForUI = true;
        }
        else
        {
            GM.waitForUI = false;
            StartCoroutine(Bump());

        }


        // if movement stopped
        if (GM.sceneDone)
        {
            // buttons active
            buttons.SetActive(true);
        }

    }

    public void Narrarate()
    {
        // if N not active, N[i] go
        if (!nActive)
        {
            nActive = true;

            N[nCounter].gameObject.SetActive(true);

            Debug.Log("S6 UI MANAGER: setting N " + nCounter + " to active, dcount is: " + dCounter + "nCount is: " + nCounter);

            // set to inactive
            // update counter
        }
        //yield return new WaitForSecondsRealtime(waitTime);
        if (dCounter == D.Length)
        {
            Debug.Log("S6 UI MANAGER: DAN FINSHED SPEAKING, Dcount = " + dCounter);
            Debug.Log("S6 UI MANAGER: dlength section setting N " + nCounter + " to inactive, dcount is: " + dCounter + "nCount is: " + nCounter);
            N[nCounter].gameObject.SetActive(false);
            nActive = false;
            nCounter++;
            dCounter++; // inc dcounter
        }

        if ((dCounter == D.Length + 1) && GM.bump == true)
        {
            Debug.Log("S6 UI MANAGER:UI GOT BUMPED");
            Debug.Log("S6 UI MANAGER: bump section setting N " + nCounter + " to inactive, dcount is: " + dCounter + "nCount is: " + nCounter);
            N[nCounter].gameObject.SetActive(false);
            nActive = false;
            dCounter++;
            nCounter++;
        }
        else
        {
            Debug.Log("UI waiting for bump..");
        }

    }

    IEnumerator Speak()
    {
        // if no one going, go D(i + 1), set going
        if (!dActive)
        {
            dActive = true;
            D[dCounter].gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(waitTime);
            D[dCounter].gameObject.SetActive(false);

            // when finished, set to no one going
            dActive = false;
            Debug.Log("S6 UI MANAGER:D active = " + dActive);
            // increment dcount
            dCounter++;
            Debug.Log("S6 UI MANAGER:D counter = " + dCounter);
        }


    }

    IEnumerator Bump()
    {

        yield return new WaitForSecondsRealtime(7);
        GM.waitToBump = false;
    }

}
    

