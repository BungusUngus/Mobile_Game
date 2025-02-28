using UnityEngine;

public class Ball : MonoBehaviour, IStop, IRestart
{
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    private bool _isMoving = true;
    private bool _inputFound;

    private Transform _followTransform;

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

    private bool CheckInput()
    {
        if (Input.touchCount > 1)
            return true;

#if UNITY_EDITOR
        //right mouse button to stand in for 2 finger touch
        return Input.GetMouseButton(1);
#endif

        //we need to return false if neither of the above pass
        return false;
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
        //refresh every frame whether or not to hold the ball
        _inputFound = CheckInput();

        if (!_isMoving)
        {
            //maintain our y position but follow the paddle's x position
            transform.position = new Vector3(_followTransform.position.x, transform.position.y, 0);

            //if the input is released, the ball should start moving again
            if (!_inputFound)
            {
                _isMoving = true;
                rb.linearVelocity = Vector2.down * speed;
            }

            //if we're not suppose to move, we should stop here
            return;
        }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if the collision has a rigidbody, AND we can get the paddle component from the rigidbody
        if (collision.rigidbody?.GetComponent<Paddle>())
        {
            //halt movement and follow the paddle
            if (_inputFound)
            {
                _isMoving = false;
                rb.linearVelocity = Vector2.zero;
                _followTransform = collision.transform;
            }


        }
    }
}
