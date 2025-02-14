using System;

public class EventController
{
    // Non-generic version (No parameters)
    public event Action BaseEvent;
    public void Invoke() => BaseEvent?.Invoke(); // Invokes the event, triggering all listeners.
    public void AddListener(Action listener) => BaseEvent += listener; // Adds a listener to the event.
    public void RemoveListener(Action listener) => BaseEvent -= listener; // Removes a listener from the event
}

public class EventController<T>
{
    // Generic version (1 parameter)
    public event Action<T> BaseEvent;
    public void Invoke(T type) => BaseEvent?.Invoke(type); // Invokes the event with a single parameter.
    public void AddListener(Action<T> listener) => BaseEvent += listener; 
    public void RemoveListener(Action<T> listener) => BaseEvent -= listener;
}

public class EventController<T1, T2>
{
    // Generic version (2 parameters)
    public event Action<T1, T2> BaseEvent;
    public void Invoke(T1 param1, T2 param2) => BaseEvent?.Invoke(param1, param2); // Invokes the event with two parameters.
    public void AddListener(Action<T1, T2> listener) => BaseEvent += listener;
    public void RemoveListener(Action<T1, T2> listener) => BaseEvent -= listener;
}