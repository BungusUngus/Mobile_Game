using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;

    [SerializeField] private float jumpPower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
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
}
