using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField]
    private float turnSpeed = 5.0f;
    AudioSource audioSource;
    [SerializeField]
    private float maintTrust = 100f;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("ok");
                break;
            case "Fuel":
                print("Fuel up");
                    break;
            default:
                print("Dead");
                break;
               
             
        }
    }


    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * maintTrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
          
        }
    }
    private void Rotate()
    {
        rigidBody.freezeRotation = true;//take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);

        }

        rigidBody.freezeRotation = false; // resume physics control
    }
}


