using UnityEngine;

public class GamePiece : MonoBehaviour, ITouchable
{
    public void OnTouchBegin(Vector3 touchPosition)
    {
        
    }

    public void OnTouchEnd(Vector3 touchPosition)
    {
        
    }

    public void OnTouchStay(Vector3 touchPosition)
    {
        //get a world space position based on the touch input
       Vector3 newPosition = ScreenInteractionController.camera.ScreenToWorldPoint(touchPosition);

        //maintain our z position 
        newPosition.z = transform.position.z;

        //apply the new position
        transform.position = newPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
