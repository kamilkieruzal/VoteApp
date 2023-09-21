using Prism.Events;
using System;
using System.Linq;
using System.Windows;
using VoteApp.DatabaseContext;
using VoteApp.Events;
using VoteApp.Exceptions;
using VoteApp.Interfaces.Interfaces;
using VoteApp.Models;

namespace VoteApp.Services
{
    public class VoterService : IVoterService
    {
        private readonly VoteAppDbContext dbContext;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageBoxService messageBoxService;

        public VoterService(
            VoteAppDbContext voteAppDbContext, 
            IEventAggregator eventAggregator, 
            IMessageBoxService messageBoxService)
        {
            dbContext = voteAppDbContext;
            this.eventAggregator = eventAggregator;
            this.messageBoxService = messageBoxService;
        }

        public bool TryAdd(string firstName, string surname)
        {
            var fullName = firstName + " " + surname;
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname))
                return false;
            if (dbContext.Voters.Any(x => x.FullName == fullName))
                return false;

            dbContext.Voters.Add(new Voter { FullName = fullName });
            dbContext.SaveChanges();

            eventAggregator.GetEvent<AddedVoterEvent>().Publish();
            return true;
        }

        public void VoteByVoter(string fullName)
        {
            try
            {
                dbContext.Voters.Single(x => x.FullName == fullName).HasVoted = true;
            }
            catch (Exception ex)
            {
                messageBoxService.Show($"Error while trying to count {fullName} vote", "Warning");
                throw new VoteException();
            }
        }
    }
}
