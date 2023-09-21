namespace VoteApp.Interfaces.Interfaces
{
    public interface IMessageBoxService
    {
        void Show(string message);
        void Show(string message, string title);
    }
}
