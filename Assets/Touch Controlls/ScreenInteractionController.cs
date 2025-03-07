using UnityEngine;

public class ScreenInteractionController : MonoBehaviour
{
    [SerializeField] private int fingerID = -1;

    private ITouchable currentTouchable;

    private static Camera _camera;

    new public static Camera camera => _camera;

    private Vector3 touchPositionLast;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouch();
    }
    private void CheckTouch()
    {
        //if we find a touch
        if(TouchDistributor.TryGetTouch(fingerID, out Touch touch))
        {
            //make sure we've updated our Finger ID to our touch
            fingerID = touch.fingerId;

            //try to find a touchable if we need to
            if(currentTouchable == null)
            {
                CastTouch(touch.position);
            }
            else
            {
                //else we'll update our current touchable
                ManageTouch(touch.position);
            }

            //we found a touch so stop here
            return;
        }

        //if we don't find a touch
        NoTouch();
    }

    private void CastTouch(Vector3 touchPosition)
    {
        Ray touchRay = camera.ScreenPointToRay(touchPosition);

        //if we hit something 
        if (Physics.Raycast(touchRay, out RaycastHit hit))
        {
            //and that something has a touchable script
            if(hit.transform.TryGetComponent<ITouchable>(out currentTouchable))
            {
                //begin it's touch behaviour 
                currentTouchable.OnTouchBegin(touchPosition);
            }
        }
    }

    private void ManageTouch(Vector3 touchPosition)
    {
        currentTouchable.OnTouchStay(touchPosition);
        touchPositionLast = touchPosition;
    }

    private void NoTouch()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            //we'll use -2 as a special "this is the mouse" finger ID 
            fingerID = -2; 
            if(currentTouchable == null)
            {
                CastTouch(Input.mousePosition);
            }
            else
            {
                ManageTouch(Input.mousePosition);
            }
            return;
        }
#endif

        if(currentTouchable != null)
        {
            currentTouchable.OnTouchEnd(touchPositionLast);
        }
        fingerID = -1;
        currentTouchable = null;
    }
}

public interface ITouchable
{
    public void OnTouchBegin(Vector3 touchPosition);
    public void OnTouchStay(Vector3 touchPosition);
    public void OnTouchEnd(Vector3 touchPosition);
}
