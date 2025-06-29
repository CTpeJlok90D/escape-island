using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Players;
using IngameDebugConsole;
using UnityEngine;

public struct PlayerSelector : IEnumerable<Player>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitSelector()
    {
        DebugLogConsole.AddCustomParameterType(typeof(PlayerSelector), ParseFromString);
    }

    private static bool ParseFromString(string input, out object output)
    {
        PlayerSelector result = new()
        {
            Value = input
        };

        output = result;
        return true;
    }

    public string Value;

    public static implicit operator Player[](PlayerSelector selector)
    {
        if (selector.Value is "@a")
        {
            return Player.Instances.ToArray();
        }

        if (selector.Value is "@r")
        {
            return new [] { Player.Instances.GetRandom() };
        }

        if (selector.Value is "@s")
        {
            return new[] { Player.Local };
        }

        foreach (Player player in Player.Instances)
        {
            if (player.Nickname == selector.Value)
            {
                return new[] { player };
            }
        }
        
        throw new ArgumentException($"Player {selector.Value} was not found");
    }
    
    public static implicit operator Player(PlayerSelector selector)
    {
        if (selector.Value is "@a")
        {
            return Player.Instances.First();
        }

        if (selector.Value is "@r")
        {
            return Player.Instances.GetRandom();
        }

        if (selector.Value is "@s")
        {
            return Player.Local;
        }

        foreach (Player player in Player.Instances)
        {
            if (player.Nickname == selector.Value)
            {
                return player;
            }
        }

        throw new ArgumentException($"Player {selector.Value} was not found");
    }

    public IEnumerator<Player> GetEnumerator()
    {
        return (IEnumerator<Player>)((Player[])this).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator<Player>)((Player[])this).GetEnumerator();
    }
}