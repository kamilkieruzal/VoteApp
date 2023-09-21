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
    public class CandidatesViewModel : ViewModelBase
    {
        private readonly VoteAppDbContext dbContext;
        private readonly IAddService<Candidate> addCandidateService;
        private ICommand addCandidateCommand;
        private ObservableCollection<Candidate> candidates;

        public CandidatesViewModel(
            VoteAppDbContext dbContext, 
            IAddService<Candidate> addCandidateService,
            IEventAggregator eventAggregator)
        {
            Candidates = new ObservableCollection<Candidate>(dbContext.Candidates);
            this.dbContext = dbContext;
            this.addCandidateService = addCandidateService;
            eventAggregator.GetEvent<AddedCandidateEvent>().Subscribe(RefreshCandidates);
            eventAggregator.GetEvent<VotedEvent>().Subscribe(RefreshCandidates);
        }

        public ICommand AddCandidateCommand => addCandidateCommand ??= new RelayCommand(c => true, c => OpenWindow());

        public ObservableCollection<Candidate> Candidates
        {
            get => candidates;
            set
            {
                candidates = value;
                OnPropertyChanged();
            }
        }

        public void OpenWindow()
        {
            var window = new AddWindow();
            var viewModel = new AddViewModel(addCandidateService, "candidate");

            viewModel.OnRequestClose += (sender, eventArgs) => window.Close();
            window.DataContext = viewModel;

            window.ShowDialog();
        }

        private void RefreshCandidates()
        {
            Candidates = new ObservableCollection<Candidate>(dbContext.Candidates);
        }
    }
}
