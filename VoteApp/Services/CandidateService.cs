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
    class CandidateService : ICandidateService
    {
        private readonly VoteAppDbContext dbContext;
        private readonly IEventAggregator eventAggregator;

        public CandidateService(VoteAppDbContext dbContext, IEventAggregator eventAggregator)
        {
            this.dbContext = dbContext;
            this.eventAggregator = eventAggregator;
        }

        public bool TryAdd(string firstName, string surname)
        {
            var fullName = firstName + " " + surname;
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname))
                return false;
            if (dbContext.Candidates.Any(x => x.FullName == fullName))
                return false;

            dbContext.Candidates.Add(new Candidate { FullName = fullName });
            dbContext.SaveChanges();

            eventAggregator.GetEvent<AddedCandidateEvent>().Publish();
            return true;
        }

        public void VoteOnCandidate(string fullName)
        {
            try
            {
                dbContext.Candidates.Single(x => x.FullName == fullName).Votes++;
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while trying to add vote to {fullName}.", "Warning");
                throw new VoteException();
            }
        }
    }
}
