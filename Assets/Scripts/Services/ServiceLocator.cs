using System;
using System.Collections.Generic;

/// <summary>
/// A simple Service Locator pattern implementation that allows different game services to be registered and accessed globally.
/// This provides a centralized way to retrieve dependencies without direct coupling between objects.
/// </summary>

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new(); // A dictionary that stores registered services, mapping a service type to its instance.

    // Registers a service instance under its type. If a service of the same type already exists, it will be replaced.
    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    // Retrieves a registered service of the specified type. If the service is not found, an error is logged.
    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out object service))
            return (T)service;

        return default;
    }
}