using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField]
    Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField]
    private float period = 2.0f;


    float movementFactor; // 0 for not moved, 1 for fully moved


    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
            
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon)
        {
            return; //protects against period = 0
        }

        float cycles = Time.time / period; // grows continually from 0

        const float tau = Mathf.PI * 2.0f; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = (rawSinWave / 2.0f + 0.5f);
        Vector3 offset = movementVector * movementFactor;

        transform.position = startingPos + offset;
    }
}
