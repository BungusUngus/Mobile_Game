using UnityEngine;

public class Ball : MonoBehaviour, IStop, IRestart
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    public void Restart()
    {
        rb.simulated = true;
        transform.position = Vector3.zero;

        //get a random point in a circle, normalise it, and then apply our speed
        rb.linearVelocity = Random.insideUnitCircle.normalized * speed;
    }

    public void Stop()
    {
        rb.simulated = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        //if the ball is moving faster or slower than it should...
        if(rb.linearVelocity.magnitude != speed)
        {
            //keep our direction, but fix the speed
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killzone"))
        {
            RoundManager.Singleton.EndGame();
        }
    }
}
