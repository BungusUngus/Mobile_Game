using UnityEngine;

public class Player : MonoBehaviour, IRestart
{

    private Rigidbody2D rb;

    [SerializeField] private float jumpPower;

    private RoundManager roundManager;

    private Vector3 homePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        homePosition = transform.position;
        //FindFirstObjectByType will return the first instance of the supplied class
        roundManager = FindFirstObjectByType<RoundManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!roundManager.isRoundActive)
            return;

            //MouseButton 0 is left click
            if (Input.GetMouseButtonDown(0))
            {
                Jump();
            }
    }

    private void Jump()
    {

        rb.linearVelocity = Vector2.up * jumpPower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        roundManager.EndGame();
        rb.simulated = false;
    }

    public void Restart()
    {
        transform.position = homePosition;
        rb.simulated = true;
    }
}
