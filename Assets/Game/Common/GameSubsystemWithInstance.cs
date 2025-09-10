namespace Game.Common
{
    public class GameSubsystemWithInstance<T> where T : GameSubsystem
    {
        public T Singleton => GameInstance.Singleton.GetSubsystem<T>();
    }
}