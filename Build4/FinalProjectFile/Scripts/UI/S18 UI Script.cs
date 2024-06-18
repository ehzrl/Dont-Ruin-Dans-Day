using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S : MonoBehaviour
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

        Debug.Log("In UI manager, here are all the names of the narration objects");
        // set all components to false
        for (int i = 0; i < N.Length; i++)
        {
            Debug.Log(N[i].gameObject.name);
            N[i].gameObject.SetActive(false);
        }

        Debug.Log("In UI manager, here are all the names of the dialouge objects");
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
            // Narrate
            StartCoroutine(Narrarate());
        }


        // if movement stopped
        if (GM.sceneDone)
        {
            // if dcount < size 
            if (dCounter < D.Length)
            {
                StartCoroutine(Speak());
            }

            // buttons active
            buttons.SetActive(true);
        }

    }

    IEnumerator Narrarate()
    {
        // if N not active, N[i] go
        if (!nActive)
        {
            nActive = true;

            N[nCounter].gameObject.SetActive(true);
            nCounter++;

            Debug.Log("UI MANAGER NARRATE: dcount is: " + dCounter);

            // set to inactive
            // update counter
        }

        //yield return new WaitForSecondsRealtime(waitTime);
        if (dCounter >= D.Length && nCounter < N.Length)
        {
            N[nCounter].gameObject.SetActive(false);
            nActive = false;
            dCounter++; // inc dcounter

            yield return new WaitForSecondsRealtime(waitTime);
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
            Debug.Log("UI Manager D active = " + dActive);
            // increment dcount
            dCounter++;
            Debug.Log("UI Manager D counter = " + dCounter);
        }

    }



}
