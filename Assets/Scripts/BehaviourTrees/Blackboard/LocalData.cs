using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalData
{
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    public T Get<T>(string key)
    {
        if (variables.ContainsKey(key))
        {
            return (T)variables[key];
        }
        else
        {
            Debug.LogError("Key " + key + " does not contain a value");
            return default(T);
        }
    }

    public T Get<T>(BlackBoardProperty<T> property)
    {
        if (variables.ContainsKey(property.Name))
        {
            variables[property.Name] = property.Get();
        }

        return (T)variables[property.Name];
    }

    public void Set<T>(BlackBoardProperty<T> property, T value)
    {
        variables[property.Name] = value;
    }

    public bool Set<T>(string key, T value)
    {
        if (variables.ContainsKey(key))
        {
            if (variables[key].GetType() == typeof(T))
            {
                variables[key] = value;
                return true;
            }
            else
            {
                Debug.LogError("Key " + key + " is not of type " + typeof(T));
                return false;
            }
        }
        else
        {
            Debug.LogError("Key " + key + " does not contain a value");
            return false;
        }
    }

    public void Add<T>(BlackBoardProperty<T> property)
    {
        variables.Add(property.Name, property.value);
    }

    public void Add<T>(Func<BlackBoardProperty<T>> constructor)
    {
        BlackBoardProperty<T> temp = constructor();

        variables.Add(temp.Name, temp.value);
    }
}

public class BlackBoardProperty<T>
{
    public string Name { get; private set; }
    public T value { get; private set; }

    public BlackBoardProperty(string name, T value)
    {
        this.Name = name;
        this.value = value;
    }

    public T Get()
    {
        return value;
    }
}