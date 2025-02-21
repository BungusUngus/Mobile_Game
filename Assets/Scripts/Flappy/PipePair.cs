using UnityEngine;

public class PipePair : MonoBehaviour, IStop, IRestart
{

    private Rigidbody2D rb;

    [SerializeField] private float speed;

    public void Restart()
    {
        Destroy(gameObject);
    }

    public void Stop()
    {
        rb.simulated = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * speed; // (-1, 0) * speed
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
