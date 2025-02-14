using System;

public class EventController
{
    // Non-generic version (No parameters)
    public event Action BaseEvent;
    public void Invoke() => BaseEvent?.Invoke();
    public void AddListener(Action listener) => BaseEvent += listener;
    public void RemoveListener(Action listener) => BaseEvent -= listener;
}

public class EventController<T>
{
    // Generic version (1 parameter)
    public event Action<T> BaseEvent;
    public void Invoke(T type) => BaseEvent?.Invoke(type);
    public void AddListener(Action<T> listener) => BaseEvent += listener;
    public void RemoveListener(Action<T> listener) => BaseEvent -= listener;
}

public class EventController<T1, T2>
{
    // Generic version (2 parameters)
    public event Action<T1, T2> BaseEvent;
    public void Invoke(T1 param1, T2 param2) => BaseEvent?.Invoke(param1, param2);
    public void AddListener(Action<T1, T2> listener) => BaseEvent += listener;
    public void RemoveListener(Action<T1, T2> listener) => BaseEvent -= listener;
}