namespace VoteApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(
            CandidatesViewModel candidatesViewModel, 
            VotersViewModel votersViewModel,
            VotePanelViewModel votePanelViewModel)
        {
            CandidatesViewModel = candidatesViewModel;
            VotersViewModel = votersViewModel;
            VotePanelViewModel = votePanelViewModel;
        }

        public CandidatesViewModel CandidatesViewModel { get; }
        public VotersViewModel VotersViewModel { get; }
        public VotePanelViewModel VotePanelViewModel { get; }
    }
}
