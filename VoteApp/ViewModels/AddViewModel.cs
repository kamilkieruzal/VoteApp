using System;
using System.Windows;
using System.Windows.Input;
using VoteApp.Commands;
using VoteApp.Interfaces.Interfaces;
using VoteApp.Interfaces.Models;

namespace VoteApp.ViewModels
{
    public class AddViewModel : ViewModelBase
    {
        private string name;
        private string surname;
        private ICommand submitCommand;
        private readonly string personType;
        private readonly IMessageBoxService messageBoxService;
        private readonly IAddService<PersonEntity> addService;

        public AddViewModel(
            IAddService<PersonEntity> addService, 
            string personType,
            IMessageBoxService messageBoxService)
        {
            this.personType = personType;
            this.messageBoxService = messageBoxService;
            this.addService = addService;
        }

        public event EventHandler OnRequestClose;

        public ICommand SubmitCommand => submitCommand ??= new RelayCommand(c => true, c => SubmitAndClose());

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged();
            }
        }

        public string TitleText => $"Add new {personType}";

        public void SubmitAndClose()
        {
            if (!addService.TryAdd(Name, Surname))
            {
                var capitalizedPersonType = char.ToUpper(personType[0]) + personType.Substring(1);
                messageBoxService.Show($"{capitalizedPersonType} has empty fields or already exists.", "Warning");
            }

            OnRequestClose(this, new EventArgs());
        }
    }
}
