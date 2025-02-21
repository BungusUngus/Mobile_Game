using UnityEngine;

public class Paddle : MonoBehaviour, IRestart
{
    [SerializeField] private float speed;

    public void Restart()
    {
        //recentre the paddle
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (!RoundManager.Singleton.isRoundActive) 
            return;

        Vector2 point = new Vector2();

        if (!Input.GetMouseButton(0))
            return;

        //take the mouse position on the screen, and convert it to a world position
        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //target the left/right position of the input, without moving any other axis
        Vector3 target = new Vector3(point.x, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
