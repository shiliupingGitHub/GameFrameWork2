namespace Game.Core.UI
{
    public class UIPathAttribute : System.Attribute
    {
        public string Path { get; set; }
        public bool WithErrorLog = true;
        public UIPathAttribute(string path)
        {
            Path = path;
        }
    }
}