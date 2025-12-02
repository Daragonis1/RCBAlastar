using System;
using System.Collections.Generic;

public abstract class GameEvent {}

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _subs = new();

    public static void Subscribe<T>(Action<T> handler) where T : GameEvent
    {
        var t = typeof(T);
        if (!_subs.TryGetValue(t, out var list))
            _subs[t] = list = new List<Delegate>();
        list.Add(handler);
    }

    public static void Publish<T>(T evt) where T : GameEvent
    {
        if (_subs.TryGetValue(typeof(T), out var list))
        {
            foreach (var del in list.ToArray())
                ((Action<T>)del)(evt);
        }
    }
}
