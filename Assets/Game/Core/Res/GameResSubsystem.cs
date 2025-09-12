using Game.Core.Common;
using UnityEngine;

namespace Game.Core.Res
{
    public class GameResSubsystem : GameSubsystemWithInstance<GameResSubsystem>
    {
        public T LoadAssetSync<T>(string path) where T : UnityEngine.Object
        {
            return null;
        }

        public GameObject InstanceGameObject(string path)
        {
            return null;
        }
        public void RecycleGameObject(string path)
        {
        }
    }
}