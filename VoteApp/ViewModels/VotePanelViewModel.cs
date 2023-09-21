using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using VoteApp.Commands;
using VoteApp.DatabaseContext;
using VoteApp.Events;
using VoteApp.Interfaces.Interfaces;
using VoteApp.Models;
using VoteApp.Services;

namespace VoteApp.ViewModels
{
    public class VotePanelViewModel : ViewModelBase
    {
        private readonly VoteAppDbContext voteAppDbContext;
        private readonly IVoteService voteService;
        private readonly IMessageBoxService messageBoxService;
        private ICommand voteCommand;
        private ObservableCollection<Voter> voters;
        private ObservableCollection<Candidate> candidates;
        private Candidate selectedCandidate;
        private Voter selectedVoter;

        public VotePanelViewModel(
            VoteAppDbContext voteAppDbContext,
            IEventAggregator eventAggregator,
            IVoteService voteService,
            IMessageBoxService messageBoxService)
        {
            this.voteAppDbContext = voteAppDbContext;
            this.voteService = voteService;
            this.messageBoxService = messageBoxService;
            Voters = new ObservableCollection<Voter>(voteAppDbContext.Voters.Where(x => !x.HasVoted));
            Candidates = new ObservableCollection<Candidate>(voteAppDbContext.Candidates);
            eventAggregator.GetEvent<AddedCandidateEvent>().Subscribe(RefreshCandidates);
            eventAggregator.GetEvent<AddedVoterEvent>().Subscribe(RefreshVoters);
            eventAggregator.GetEvent<VotedEvent>().Subscribe(RefreshAll);
        }

        public ICommand VoteCommand => voteCommand ??= new RelayCommand(c => true, c => Vote());

        public Candidate SelectedCandidate
        {
            get { return selectedCandidate; }
            set
            {
                selectedCandidate = value;
                OnPropertyChanged();
            }
        }

        public Voter SelectedVoter
        {
            get { return selectedVoter; }
            set
            {
                selectedVoter = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Voter> Voters
        {
            get { return voters; }
            set
            {
                voters = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Candidate> Candidates
        {
            get { return candidates; }
            set
            {
                candidates = value;
                OnPropertyChanged();
            }
        }

        private void RefreshVoters()
        {
            Voters = new ObservableCollection<Voter>(voteAppDbContext.Voters.Where(x => !x.HasVoted));
        }

        private void RefreshCandidates()
        {
            Candidates = new ObservableCollection<Candidate>(voteAppDbContext.Candidates);
        }

        private void RefreshAll()
        {
            RefreshCandidates();
            RefreshVoters();
        }

        private void Vote()
        {
            if (SelectedVoter == null || SelectedCandidate == null)
            {
                messageBoxService.Show("Please select valid voter and candidate.", "Warning");
                return;
            }

            voteService.Vote(SelectedVoter.FullName, selectedCandidate.FullName);

            SelectedCandidate = null;
            SelectedVoter = null;
        }
    }
}
