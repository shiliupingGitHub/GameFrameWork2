using System.Collections.Generic;
using UnityEngine;

namespace Game.Common
{
    public class GameInstance
    {
        public static GameInstance Singleton { get; private set; }

        public GameInstance()
        {
            Singleton = this;
        }

        System.Type[] _allTypes;
        private readonly List<System.Type> _gameSubsystemTypes = new();
        private readonly List<System.Type> _worldSubsystemTypes = new();
        private Dictionary<System.Type, GameSubsystem> _gameSubsystems = new();
        List<World> _worlds = new();

        public T GetSubsystem<T>() where T : GameSubsystem
        {
            return _gameSubsystems.GetValueOrDefault(typeof(T)) as T;
        }

        public void Load()
        {
            _allTypes = typeof(GameInstance).Assembly.GetTypes();
            foreach (var type in _allTypes)
            {
                if (type.IsSubclassOf(typeof(GameSubsystem)))
                {
                    _gameSubsystemTypes.Add(type);
                    var gs = System.Activator.CreateInstance(type) as GameSubsystem;
                    _gameSubsystems[type] = gs;
                    gs?.OnLoad();
                }

                if (type.IsSubclassOf(typeof(WorldSubsystem)))
                {
                    _worldSubsystemTypes.Add(type);
                }
            }

            foreach (var s in _gameSubsystems)
            {
                s.Value.OnLateLoad();
            }
        }

        public World CreateWorld(string worldName)
        {
            var world = new World(worldName);
            _worlds.Add(world);
            world.Load(_worldSubsystemTypes);
            return world;
        }
    }
}