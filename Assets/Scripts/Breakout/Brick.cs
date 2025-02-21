using UnityEngine;

public class Brick : MonoBehaviour, IRestart
{
    public void Restart()
    {
        gameObject.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        gameObject.SetActive(false);
    }
}
