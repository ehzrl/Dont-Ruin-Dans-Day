using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //Parse scene number
    public int ParseSceneNum()
    {
        string numStr = SceneManager.GetActiveScene().name.Substring(1); // get number of scene "S#"
        int num;

        if(Int32.TryParse(numStr, out num))
        {
            Debug.Log("Current scene: " + num);
        }
        else
        {
            Debug.Log("Couldn't parse ");
            num = -1;
        }

        return num;
    }


    [ContextMenu("Next Scene")]
    public void NextScene()
    {
        int num = ParseSceneNum();
        // Check if scene parsed correctly
        if (num != -1)
        {
            num += 1; // increment num
            String scene = "S" + num; // concatenate to produce next scene

            Debug.Log("Switching to Scene " + scene);

            // load scene
            SceneManager.LoadScene(scene);
        }
        
    }

    public void SecondNextScene()
    {
        int num = ParseSceneNum();
        // Check if scene parsed correctly
        if (num != -1)
        {
            num += 2; // increment num
            String scene = "S" + num; // concatenate to produce next scene

            Debug.Log("Switching to Scene " + scene);

            // load scene
            SceneManager.LoadScene(scene);
        }
    }

    public void ThirdNextScene() 
    {
        int num = ParseSceneNum();
        // Check if scene parsed correctly
        if (num != -1)
        {
            num += 3; // increment num
            String scene = "S" + num; // concatenate to produce next scene

            Debug.Log("Switching to Scene " + scene);

            // load scene
            SceneManager.LoadScene(scene);
        }
    }
    

    // start game over
    //[ContextMenu("Restart Game")]
    public void RestartGame()
    {
        SceneManager.LoadScene("S2");
    }
    
    [ContextMenu("Restart Game")]
    // go to home screen
    public void ToTitleScreen()
    {
        SceneManager.LoadScene("S0");
    }

}
