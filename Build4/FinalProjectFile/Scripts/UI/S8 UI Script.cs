using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S8UIScript : MonoBehaviour
{
    public GameObject buttons;
    public GameObject narration;
    public Text[] N;
    public Boolean nActive;
    public int nCounter;
    public int waitTime = 7; // wait to change dialouge

    // Start is called before the first frame update
    void Start()
    {
        // Initalize Objects
        buttons = GameObject.Find("Buttons");
        narration = GameObject.Find("N1");
        

        // set all to not visable
        narration.SetActive(true);
        buttons.SetActive(false);

        // initalize arrays
        N = narration.GetComponentsInChildren<Text>();

        // initalize bools
        nActive = false;

        // initalize counters
        nCounter = 0;

        Debug.Log("In UI manager, here are all the names of the narration objects");
        // set all components to false
        for (int i = 0; i < N.Length; i++)
        {
            Debug.Log(N[i].gameObject.name);
            N[i].gameObject.SetActive(false);
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
            yield return new WaitForSecondsRealtime(waitTime);
            if(nCounter < N.Length - 1)
            {
                N[nCounter].gameObject.SetActive(false);
                nActive = false;
                
            }
            nCounter++;


            // set to inactive
            // update counter
        }

    }

  /*  IEnumerator Speak()
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
  */



}
