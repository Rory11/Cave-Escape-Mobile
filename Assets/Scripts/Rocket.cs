
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    [SerializeField]
    private float turnSpeed = 5.0f;
    [SerializeField]
    private float maintTrust = 100f;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
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
                state = State.Transcending;
                Invoke("LoadNextLevel", 2.0f);  //     parameterise time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 2.0f);
                break;
               
             
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * maintTrust);
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


