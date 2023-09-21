using Prism.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VoteApp.Commands;
using VoteApp.DatabaseContext;
using VoteApp.Events;
using VoteApp.Interfaces.Interfaces;
using VoteApp.Models;
using VoteApp.Windows;

namespace VoteApp.ViewModels
{
    public class VotersViewModel : ViewModelBase
    {
        private readonly VoteAppDbContext dbContext;
        private readonly IAddService<Voter> addVoterService;
        private ICommand addVoterCommand;
        private ObservableCollection<Voter> voters;

        public VotersViewModel(
            VoteAppDbContext dbContext, 
            IAddService<Voter> addVoterService,
            IEventAggregator eventAggregator)
        {
            voters = new ObservableCollection<Voter>(dbContext.Voters);
            this.dbContext = dbContext;
            this.addVoterService = addVoterService;
            eventAggregator.GetEvent<AddedVoterEvent>().Subscribe(RefreshVoters);
            eventAggregator.GetEvent<VotedEvent>().Subscribe(RefreshVoters);
        }

        public ICommand AddVoterCommand => addVoterCommand ??= new RelayCommand(c => true, c => OpenWindow());

        public ObservableCollection<Voter> Voters
        {
            get => voters;
            set
            {
                voters = value;
                OnPropertyChanged();
            }
        }

        public void OpenWindow()
        {
            var window = new AddWindow();
            var viewModel = new AddViewModel(addVoterService, "voter");

            viewModel.OnRequestClose += (sender, eventArgs) => window.Close();
            window.DataContext = viewModel;

            window.ShowDialog();
        }

        private void RefreshVoters()
        {
            Voters = new ObservableCollection<Voter>(dbContext.Voters);
        }
    }
}
