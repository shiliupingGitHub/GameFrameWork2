namespace Game.Core.Common
{
    public interface ISubsystem
    {
        public void OnLoad();
        public void OnLateLoad();
    }
}