using UnityEngine;

public class PipePair : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] private float speed;

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
