using UnityEngine;

[DisallowMultipleComponent]
public class moveComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    Vector3 movePosition;

    [SerializeField]
    float moveSpeed;

    [SerializeField][Range(0, 1)] float moveProgress;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time * moveSpeed, 1);

        Vector3 offset = movePosition * moveProgress;
        transform.position = startPosition + offset;
    }
}
