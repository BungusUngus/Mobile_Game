using UnityEngine;

public static class TouchDistributor
{
    public static bool TryGetTouch(int touchID , out Touch touchFound)
    {
        //loop through all our touches currently on the screen
        //and try to find a new touch, or maintain a current touch
        foreach (Touch touch in Input.touches)
        {
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
