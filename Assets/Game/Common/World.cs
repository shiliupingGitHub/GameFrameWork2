using System.Collections.Generic;

namespace Game.Common
{
    public class World
    {
        public string Name { get; private set; }
        private Dictionary<System.Type, WorldSubsystem> _worldSubsystems = new();

        public World(string worldName)
        {
            Name = worldName;
        }

        public T GetSubsystem<T>() where T : WorldSubsystem
        {
            return _worldSubsystems.GetValueOrDefault(typeof(T)) as T;
        }

        public void Load(List<System.Type> st)
        {
            foreach (var t in st)
            {
                var worldSubsystem = System.Activator.CreateInstance(t) as WorldSubsystem;
                _worldSubsystems[t] = worldSubsystem;
                worldSubsystem?.OnLoad();
            }

            foreach (var s in _worldSubsystems)
            {
                s.Value.OnLateLoad();
            }
        }
    }
}