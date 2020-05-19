
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class RocketMobile : MonoBehaviour
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
    AudioClip movementSound = null;
    [SerializeField]
    AudioClip deathSound = null;
    [SerializeField]
    AudioClip successSound = null;

    [SerializeField]
    ParticleSystem movementParticles = null;
    [SerializeField]
    ParticleSystem deathParticles = null;
    [SerializeField]
    ParticleSystem successParticles = null;

    private Vector3 startPosition;
    private Quaternion startRotation;
    [SerializeField]
    private SphereCollider playerHit;

    //debug var
    bool collisionsDisabled = false;

    private float screenWidth;




    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        playerHit = GetComponent<SphereCollider>();
        UIManager.LifeCounter = 3;
        screenWidth = Screen.width / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            TouchControl();
        }
        //to have only if debuging game
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        if(Keyboard.current.lKey.isPressed)
        {
            LoadNextLevel();
        }
        //else if (Input.GetKeyDown(KeyCode.C))
        else if (Keyboard.current.cKey.isPressed)
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
        isTransitioning = true;
        playerHit.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        if (lives >= 1)
        {
            UIManager.LifeCounter -= 1;
            Invoke("LifeLost", levelLoadDelay);
        }
        else
        {
            UIManager.LifeCounter = 0;
            Invoke("LoadFirstLevel", levelLoadDelay);
        }
    }

    private void LifeLost()
    {
        ResetPosition();
        deathParticles.Stop();
        playerHit.enabled = true;
        isTransitioning = false;
    }

    private void ResetPosition()
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
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
    
    private void StopMovementAudio()
    {
        audioSource.Stop();
        movementParticles.Stop();
    }

    private void PlayMovementAudio()
    {
        if (!audioSource.isPlaying)
        {
            movementParticles.Play();
            audioSource.PlayOneShot(movementSound);
        }
    }


    //MOVEMENT

    void TouchControl()
    {
        rigidBody.angularVelocity = Vector3.zero;  //remove rotation due to physics
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < screenWidth)
            {
                RotateLeft();
            }
            else if (touch.position.x > screenWidth)
            {
                RotateRight();
            }
        }
    }
     
    private void RespondToThrustInput()
    {
        // if (Input.GetKey(KeyCode.Space))
        if (Input.GetMouseButton(0))
           // if (Keyboard.current.spaceKey.isPressed)
        {
            ApplyThrust();
        }

        else
        {
            StopThrust();
        }
    }

    public void StopThrust()
    {
        StopMovementAudio();
    }


    public void RotateRight()
    {
        transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
    }

    public void RotateLeft()
    {
        transform.Rotate(-Vector3.forward * turnSpeed * Time.deltaTime);
    }

    public void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * maintThrust * Time.deltaTime);
        PlayMovementAudio();
    }

}  


