using System.Linq;
using UnityEngine;

public static class TouchDistributor
{
    public static bool TryGetTouch(int touchID , out Touch touchFound, params int[] excludeIDs)
    {
        //loop through all our touches currently on the screen
        //and try to find a new touch, or maintain a current touch
        foreach (Touch touch in Input.touches)
        {
            Debug.Log("Checking input finger ID " + touchID + " against " + touch.fingerId);
            //if the currently iterated touch is already in use, but NOT the finger we're currently trying to update, go to the next touch
            if (excludeIDs.Contains(touch.fingerId) && touchID != touch.fingerId)
            {
                Debug.Log("excluded finger id" + touch.fingerId);
                continue;
            }

            //if we have no current touch, or we find our maintained touch
            if(touchID == 1 || touch.fingerId == touchID)
            {
                touchFound = touch;
                return true;
            }
        }
        // if we checked all the touches, and none were a match, we have no touch
        //give our out value a blank value
        touchFound = new Touch();
        return false;
    }
}
