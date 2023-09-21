using System.Windows;
using VoteApp.Interfaces.Interfaces;

namespace VoteApp.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message)
        {
            MessageBox.Show(message);
        }

        public void Show(string message, string title)
        {
            MessageBox.Show(message, title);
        }
    }
}
