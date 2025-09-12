namespace Game.Core.Common
{
    public class GameSubsystemWithInstance<T> : GameSubsystem where T : GameSubsystem
    {
        public static T Singleton => GameInstance.Singleton.GetSubsystem<T>();
    }
}