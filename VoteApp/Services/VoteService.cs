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
        private readonly IMessageBoxService messageBoxService;

        public VoteService(
            IVoterService voterService, 
            ICandidateService candidateService,
            VoteAppDbContext dbContext,
            IEventAggregator eventAggregator,
            IMessageBoxService messageBoxService)
        {
            this.voterService = voterService;
            this.candidateService = candidateService;
            this.dbContext = dbContext;
            this.eventAggregator = eventAggregator;
            this.messageBoxService = messageBoxService;
        }

        public void Vote(string voterFullName, string candidateFullName)
        {
            try
            {
                voterService.VoteByVoter(voterFullName);
                candidateService.VoteOnCandidate(candidateFullName);
                dbContext.SaveChanges();

                eventAggregator.GetEvent<VotedEvent>().Publish();
                messageBoxService.Show($"Successfully submitted \"{voterFullName}\" vote on \"{candidateFullName}\"!", "Information");
            }
            catch (VoteException) 
            {
                messageBoxService.Show("Vote cancelled");
            }
        }
    }
}
