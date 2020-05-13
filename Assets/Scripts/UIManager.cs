using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static int LifeCounter = 3;
    Text LifeNumber;

    // Start is called before the first frame update
    void Start()
    {
        LifeNumber = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeNumber.text = "Lives: " + LifeCounter;
    }
}
