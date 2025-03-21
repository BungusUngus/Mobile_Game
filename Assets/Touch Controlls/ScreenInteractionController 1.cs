using System.Collections.Generic;
using UnityEngine;

public class ScreenInteractionController : MonoBehaviour
{
    private ScreenInteraction[] currentInteraction = new ScreenInteraction[5];

    private static Camera _camera;

    new public static Camera camera => _camera;

    private Vector3[] touchPositionLast = new Vector3[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        foreach (var interaction in currentInteraction)
        {
            if (interaction)
            print(interaction.fingerID);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            CheckTouch(i, GetCurrentFingerIDs());
        }
    }

    private void CheckTouch(int index, int[] excludeFingerIDs)
    {
        int fingerID = currentInteraction[index] != null ? currentInteraction[index].fingerID : -1;
        //if we find a touch
        if (TouchDistributor.TryGetTouch(fingerID, out Touch touch, excludeFingerIDs))
        {
            //if we're starting a new interaction,
            if (currentInteraction[index] == null)
            {
                //construct one of our classes
                currentInteraction[index] = new ScreenInteraction(touch.fingerId, touch.position);
            }
            else
            {
                currentInteraction[index].Poll(touch.position);
                touchPositionLast[index] = touch.position;
            }

            //try to find a touchable if we need to
            if (currentInteraction[index].touchable == null)
            {
                CastTouch(touch.position, index);
            }
            else
            {
                //else we'll update our current touchable
                ManageTouch(touch.position, index);
            }

            //we found a touch, so stop here
            return;
        }

        //if we don't find a touch
        NoTouch(index);
    }

    private void CastTouch(Vector3 touchPosition, int index)
    {
        Ray touchRay = camera.ScreenPointToRay(touchPosition);

        //if we hit something
        if (Physics.Raycast(touchRay, out RaycastHit hit))
        {
            //and that something has a touchable script
            if (hit.transform.TryGetComponent<ITouchable>(out ITouchable currentTouchable))
            {
                //begin it's touch behaviour
                currentInteraction[index].TryAddTouchable(currentTouchable);
                currentTouchable.OnTouchBegin(touchPosition);
            }
        }
    }

    private void ManageTouch(Vector3 touchPosition, int index)
    {
        currentInteraction[index].touchable.OnTouchStay(touchPosition);
    }

    private void NoTouch(int index)
    {
#if UNITY_EDITOR
        if (Input.touchCount == 0 && Input.GetMouseButton(0) && index == 0)
        {
            if (currentInteraction[index] == null)
            {
                currentInteraction[index] = new ScreenInteraction(-2, Input.mousePosition);
            }
            else
            {
                currentInteraction[index].Poll(Input.mousePosition);
                touchPositionLast[index] = Input.mousePosition;
            }

            if (currentInteraction[index].touchable == null)
            {
                CastTouch(Input.mousePosition, index);
            }
            else
            {
                ManageTouch(Input.mousePosition, index);
            }
            return;
        }
#endif

        if (currentInteraction[index] != null)
        {
            //End the current interaction
            currentInteraction[index].End(touchPositionLast[index]);

            //if we have an interaction, AND we're managing a touchable (aka drag)
            if (currentInteraction[index].touchable != null)
            {
                currentInteraction[index].touchable.OnTouchEnd(touchPositionLast[index]);
            }
            else
            {
                //try to swipe
                if (ScreenInteraction.Swipe.Try(currentInteraction[index], out ScreenInteraction.Swipe swipe))
                {
                    Debug.Log($"Did a swipe, from {swipe.start} to {swipe.end} covering distance of {swipe.distance}");
                }
                else if (ScreenInteraction.Tap.Try(currentInteraction[index], out ScreenInteraction.Tap tap))
                {
                    Debug.Log($"Did a tap at {tap.screenPosition}. In world, this is {tap.WorldPosition}");
                }
            }
        }
        currentInteraction[index] = null;
    }

    private int[] GetCurrentFingerIDs()
    {
        List<int> fingerIDs = new();

        foreach (ScreenInteraction interaction in currentInteraction)
        {
            if (interaction)
            {
                fingerIDs.Add(interaction.fingerID);
            }
        }

        for (int i = 0; i < fingerIDs.Count; i++)
        {
            print($"At position {i} Found FingerID to exclude: {fingerIDs[i]}");
        }

        return fingerIDs.ToArray();
    }
}

public interface ITouchable
{
    public void OnTouchBegin(Vector3 touchPosition);
    public void OnTouchStay(Vector3 touchPosition);
    public void OnTouchEnd(Vector3 touchPosition);
}