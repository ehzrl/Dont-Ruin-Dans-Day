using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DUIScript : MonoBehaviour
{
    public GameObject buttons;
    public GameObject narration;
    public Image[] N;
    public Boolean nActive;
    public int nCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Initalize Objects
        buttons = GameObject.Find("Buttons");
        narration = GameObject.Find("Narration");

        // set all to not visable
        narration.SetActive(true);
        buttons.SetActive(false);

        // initalize arrays
        N = narration.GetComponentsInChildren<Image>();
       

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
        if(nCounter < N.Length)
        {
            // Narrate
            Narrarate();
        }
          

        // if movement stopped
        if(!GM.sceneMoving)
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
            nCounter++;
            nActive = false;
        }

    }
   
}
