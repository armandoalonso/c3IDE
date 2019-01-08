namespace c3IDE.Windows.Interfaces
{
    public interface IWindow
    {
        string DisplayName { get; set; }
        void OnEnter();
        void OnExit();
    }
}