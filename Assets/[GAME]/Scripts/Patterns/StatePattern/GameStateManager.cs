﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scripts.Patterns.StatePattern
{
    public sealed class GameStateManager
    {
        readonly List<Type> _gameStates;

        private GameStateManager()
        {
            _gameStates = GetDerivedTypes(typeof(GameStateBase), Assembly.GetExecutingAssembly());
        }

        private static GameStateManager _instance;

        public static GameStateManager Instance()
        {
            if (_instance == null)
            {
                _instance = new GameStateManager();
            }
            return _instance;
        }

        public void PrintAll()
        {
            for (int i = 0; i < _gameStates.Count; i++)
            {
                Debug.Log(_gameStates[i].Name);
            }
        }
        
        // public TState SetState<TState>() where TState : GameStateBase
        // {
        //     _currentState = typeof(TState);
        // }
        
        private List<Type> GetDerivedTypes(Type baseType, Assembly assembly)
        {
            // Get all types from the given assembly
            Type[] types = assembly.GetTypes();
            List<Type> derivedTypes = new List<Type>();

            for (int i = 0, count = types.Length; i < count; i++)
            {
                Type type = types[i];
                if (IsSubclassOf(type, baseType))
                {
                    // The current type is derived from the base type,
                    // so add it to the list
                    derivedTypes.Add(type);
                }
            }

            return derivedTypes;
        }
        
        private bool IsSubclassOf(Type type, Type baseType)
        {
            if (type == null || baseType == null || type == baseType)
                return false;

            if (baseType.IsGenericType == false)
            {
                if (type.IsGenericType == false)
                    return type.IsSubclassOf(baseType);
            }
            else
            {
                baseType = baseType.GetGenericTypeDefinition();
            }

            type = type.BaseType;
            Type objectType = typeof(object);

            while (type != objectType && type != null)
            {
                Type curentType = type.IsGenericType ?
                    type.GetGenericTypeDefinition() : type;
                if (curentType == baseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }
}