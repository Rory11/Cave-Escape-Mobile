using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField]
    private float turnSpeed = 2.0f;
    [SerializeField]
    private float thrustPower = 10.0f;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustPower);
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
            print("Rotating Left");
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
            print("Rotating Right");
        }
    }
}
