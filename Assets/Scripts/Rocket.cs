
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;

    [SerializeField]
    private float turnSpeed = 5.0f;
    [SerializeField]
    private float maintThrust = 100f;
    [SerializeField]
    private float levelLoadDelay = 2.0f;

    bool isTransitioning = false;

    AudioSource audioSource;
    [SerializeField]
    AudioClip movementSound;
    [SerializeField]
    AudioClip deathSound;
    [SerializeField]
    AudioClip successSound;

    [SerializeField]
    ParticleSystem movementParticles;
    [SerializeField]
    ParticleSystem deathParticles;
    [SerializeField]
    ParticleSystem successParticles;

    //debug var
    bool collisionsDisabled = false;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        //to have only if debuging game
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;


        }
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0; //restart game at level 1
        }
        SceneManager.LoadScene(nextSceneIndex); // allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            PlayMovementAudio();
        }

        else
        {
            StopMovementAudio();
        }
    }

       

    private void StopMovementAudio()
    {
        audioSource.Stop();
        movementParticles.Stop();
    }


    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * maintThrust * Time.deltaTime);
    }

    private void PlayMovementAudio()
    {
        if (!audioSource.isPlaying)
        {
            movementParticles.Play();
            audioSource.PlayOneShot(movementSound);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero;  //remove rotation due to physics

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
        }

        
    }
 }  


