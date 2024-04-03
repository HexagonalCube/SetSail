using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> {}

public class GameEventListener : MonoBehaviour
{
    //reference of the gameEvent script
    public GameEvent gameEvent;

    public CustomGameEvent response;

    //this method passes the object this script is attached to as a listener
    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    //unregister this game object
    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    //this will call the unityEvent - invoke is a built-in method
    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
