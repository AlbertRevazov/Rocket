using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    enum State
    {
        Play,
        Dead,
        Finish,
    };
    [SerializeField] Text energyText;
    [SerializeField] int energyTotal = 450;
    [SerializeField] int energyApply = 45;

    [SerializeField]
    State state = State.Play;
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField]
    float rotationSpeed = 100f;

    [SerializeField]
    float flySpeed = 100f;

    [SerializeField]
    AudioClip flySound;

    [SerializeField]
    AudioClip finishSound;

    [SerializeField]
    AudioClip deadSound;

    [SerializeField]
    ParticleSystem flyParticle;

    [SerializeField]
    ParticleSystem deadParticle;

    [SerializeField]
    ParticleSystem finishParticle;

    [SerializeField]
    bool collisionOff = false;
    void Start()
    {
        print("Start");
        state = State.Play;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.Play)
        {
            Rotation();
            Launch();
        }
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionOff = !collisionOff;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Play || collisionOff)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "friendly":
                break;
            case "Finish":
                Finish();
                break;
            case "battery":
                AddEnergy(450, collision.gameObject);
                break;
            default:
                Dead();
                break;
        }

    }

    void AddEnergy(int energyCount, GameObject battery)
    {
        print(battery.GetComponent<BoxCollider>());
        energyTotal += energyCount;
        energyText.text = energyTotal.ToString();
        Destroy(battery);

    }

    void Launch()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.Space) && energyTotal > 0)
        {
            energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime);
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound);
            flyParticle.Play();
        }
        else
        {
            audioSource.Pause();
            flyParticle.Stop();
        }
        rigidBody.freezeRotation = false;
    }

    void Finish()
    {
        state = State.Finish;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticle.Play();
        Invoke("NextLevel", 2f);
    }

    void Dead()
    {
        state = State.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deadSound);
        flyParticle.Stop();
        deadParticle.Play();
        Invoke("FirstLevel", 2f);
    }

    void Rotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    void NextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        print((nextLevelIndex, SceneManager.sceneCountInBuildSettings));

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 1;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }

    void FirstLevel()
    {
        SceneManager.LoadScene(1);
    }
}
