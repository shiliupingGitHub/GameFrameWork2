using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Game.Core.Common
{
    public class GameInstance
    {
        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
           var currentLoop = PlayerLoop.GetCurrentPlayerLoop();
           
           // 创建自定义系统
           var customSystem = new PlayerLoopSystem()
           {
               type = typeof(GameInstance),
               updateDelegate = OnCustomUpdate
           };
           InsertSystem(ref currentLoop, customSystem, typeof(Update));
           PlayerLoop.SetPlayerLoop(currentLoop);
        }
        private static void InsertSystem(ref PlayerLoopSystem playerLoop, 
            PlayerLoopSystem systemToInsert, 
            Type targetSystemType)
        {
            // 查找目标系统类型的位置
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                if (playerLoop.subSystemList[i].type == targetSystemType)
                {
                    // 在目标系统的子系统中插入自定义系统
                    var subSystems = playerLoop.subSystemList[i].subSystemList.ToList();
                    subSystems.Add(systemToInsert);
                    playerLoop.subSystemList[i].subSystemList = subSystems.ToArray();
                    break;
                }
            }
        }

        static void OnCustomUpdate()
        {
           
        }
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
        public static System.Action OnQuit;

        public T GetSubsystem<T>() where T : GameSubsystem
        {
            return _gameSubsystems.GetValueOrDefault(typeof(T)) as T;
        }

        public void Load()
        {
            _allTypes = typeof(GameInstance).Assembly.GetTypes();
            foreach (var type in _allTypes)
            {
                if (type.IsSubclassOf(typeof(GameSubsystem)) && !type.IsGenericType)
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