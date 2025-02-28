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

        if (!TryGetInputPosition(out Vector3 point))
            return;

        //convert our screenspace point to a world point
        point = Camera.main.ScreenToWorldPoint(point);

        //target the left/right position of the input, without moving any other axis
        Vector3 target = new Vector3(point.x, transform.position.y, transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private bool TryGetInputPosition(out Vector3 position)
    {
        //this must be included to initialise the "out" variable
        position = new Vector3();

        //if we're getting touches, do this stuff
        if (Input.touchCount > 0)
        {
            position = Input.GetTouch(0).position;
            return true;
        }

        //if we make it here. no touch is happening
#if UNITY_EDITOR
        //if we're in editor, check for mouse controls
        if (!Input.GetMouseButton(0))
            return false;

        position = Input.mousePosition;
        return true;
#endif

        //if we get here, we're not in editor, AND we have no touch, so return false
        return false;
    }
}
