namespace Game.Common
{
    public interface ISubsystem
    {
        public void OnLoad();
        public void OnLateLoad();
    }
}