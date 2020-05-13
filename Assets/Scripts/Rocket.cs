
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
    [SerializeField]
    private int lives = 3;

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

    private Vector3 startPosition;
    private Quaternion startRotation;
    [SerializeField]
    private SphereCollider playerHit;
    


    //debug var
    bool collisionsDisabled = false;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        playerHit = GetComponent<SphereCollider>();
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
                lives--;
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
        playerHit.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        if (lives >= 1)
        {
            Invoke("LifeLost", levelLoadDelay);
        }
        else
        {
            isTransitioning = true;
            Invoke("LoadFirstLevel", levelLoadDelay);
        }
    }

    private void LifeLost()
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
        deathParticles.Stop();
        playerHit.enabled = true;
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

    private void LoadCurrentLevel()
    {
    int currentScene = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentScene);
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


