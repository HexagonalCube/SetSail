using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the radio station
[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    //listeners' ledger
    public List<GameEventListener> listeners = new List<GameEventListener>();


    public void Raise(Component sender, object data)
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }


    //adding up the listeners to the list
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))       
            listeners.Add(listener);       
    }

    //removing the listeners, so it ends the execution of the object's method
    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

}
