using Game.Core.Common;

namespace Game.Core.Res
{
    public class GameResSubsystem : GameSubsystemWithInstance<GameResSubsystem>
    {
        public T LoadAssetSync<T>(string path) where T : UnityEngine.Object
        {
            return null;
        }
    }
}