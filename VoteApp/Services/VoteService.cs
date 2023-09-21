using Prism.Events;
using System.Windows;
using VoteApp.DatabaseContext;
using VoteApp.Events;
using VoteApp.Exceptions;
using VoteApp.Interfaces.Interfaces;

namespace VoteApp.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoterService voterService;
        private readonly ICandidateService candidateService;
        private readonly VoteAppDbContext dbContext;
        private readonly IEventAggregator eventAggregator;

        public VoteService(
            IVoterService voterService, 
            ICandidateService candidateService,
            VoteAppDbContext dbContext,
            IEventAggregator eventAggregator)
        {
            this.voterService = voterService;
            this.candidateService = candidateService;
            this.dbContext = dbContext;
            this.eventAggregator = eventAggregator;
        }

        public void Vote(string voterFullName, string candidateFullName)
        {
            try
            {
                voterService.VoteByVoter(voterFullName);
                candidateService.VoteOnCandidate(candidateFullName);
                dbContext.SaveChanges();

                eventAggregator.GetEvent<VotedEvent>().Publish();
                MessageBox.Show($"Successfully submitted \"{voterFullName}\" vote on \"{candidateFullName}\"!", "Information");
            }
            catch (VoteException) 
            {
                MessageBox.Show("Vote cancelled");
            }
        }
    }
}
