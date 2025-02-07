using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private bool _isRoundActive = true;

    public bool isRoundActive => _isRoundActive;

    private static RoundManager _singleton;

    public static RoundManager singleton
    {
        //when something accesses RoundManager.singleton, it will evaluate and return whatever is in _singleton
        get => _singleton;

        //we can say "private" before "set" to make this read-only to other scripts
        private set
        {
            //value is whatever the user is trying to apply.
            //e.g. RoundManager.singleton = objectA, value stands in for objectA
            if (_singleton != null)
            {
                Debug.LogWarning("There should only be one RoundManager in the scene! The other manager has been destroyed.");
                //this would destroy objectA
                Destroy(value);

                return;
            }
            _singleton = value;
        }
    }

    private void Awake()
    {
        singleton = this;
    }

    public void NewGame()
    {
        _isRoundActive = true;
    }

    public void EndGame()
    {
        _isRoundActive = false;
    }
}
