using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipTutorial : MonoBehaviour
{

    public void skipTutorial ()
    {
        {
            SceneManager.LoadScene(5); //  skip the tutorial level and go right to level 1
        }
    }

    
}
