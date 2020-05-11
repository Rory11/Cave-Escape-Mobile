
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

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

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



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
     }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
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
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);  //     parameterise time
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * maintThrust * Time.deltaTime);
            PlayMovementAudio();
        }
    }

    private void PlayMovementAudio()
    {
        if (!audioSource.isPlaying)
        {
            movementParticles.Play();
            audioSource.PlayOneShot(movementSound);
        }
        else
        {
            movementParticles.Stop();
            audioSource.Stop();
        }
    }

    private void RespondToRotateInput()
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


