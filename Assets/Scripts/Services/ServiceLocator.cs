using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<Type, object> _services = new();

    // Registers a service instance to the locator
    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
        Debug.Log($"Service Registered: {typeof(T).Name}");
    }

    // Retrieves a registered service instance
    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            Debug.Log($"Service Retrieved: {typeof(T).Name}");
            return (T)service;
        }

        Debug.LogError($"Service Not Found: {typeof(T).Name}"); // Error log
        throw new Exception($"Service not found: {typeof(T).Name}");
    }
}
