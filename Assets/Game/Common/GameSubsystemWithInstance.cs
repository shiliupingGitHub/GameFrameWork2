namespace Game.Common
{
    public class GameSubsystemWithInstance<T> : GameSubsystem where T : GameSubsystem
    {
        public T Singleton => GameInstance.Singleton.GetSubsystem<T>();
    }
}