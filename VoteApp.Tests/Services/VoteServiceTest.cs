using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prism.Events;
using VoteApp.DatabaseContext;
using VoteApp.Events;
using VoteApp.Interfaces.Interfaces;
using VoteApp.Models;
using VoteApp.Services;

namespace VoteApp.Tests.Services
{
    [TestClass]
    public class VoteServiceTests
    {
        private VoteService voteService;
        Mock<IVoterService> voterServiceMock = new();
        Mock<ICandidateService> candidateServiceMock = new();
        Mock<IEventAggregator> eventAggregatorMock = new();
        Mock<VoteAppDbContext> dbContextMock = new();
        Mock<IMessageBoxService> messageBoxService = new();

        [TestInitialize]
        public void Setup()
        {
            var voterData = GetSampleVotersData().AsQueryable();
            var candidatesData = GetSampleCandidatesData().AsQueryable();

            var voterMockSet = new Mock<DbSet<Voter>>();
            var candidateMockSet = new Mock<DbSet<Candidate>>();
            voterMockSet.As<IQueryable<Voter>>().Setup(m => m.Provider).Returns(voterData.Provider);
            voterMockSet.As<IQueryable<Voter>>().Setup(m => m.Expression).Returns(voterData.Expression);
            voterMockSet.As<IQueryable<Voter>>().Setup(m => m.ElementType).Returns(voterData.ElementType);
            voterMockSet.As<IQueryable<Voter>>().Setup(m => m.GetEnumerator()).Returns(() => voterData.GetEnumerator());
            candidateMockSet.As<IQueryable<Candidate>>().Setup(m => m.Provider).Returns(candidatesData.Provider);
            candidateMockSet.As<IQueryable<Candidate>>().Setup(m => m.Expression).Returns(candidatesData.Expression);
            candidateMockSet.As<IQueryable<Candidate>>().Setup(m => m.ElementType).Returns(candidatesData.ElementType);
            candidateMockSet.As<IQueryable<Candidate>>().Setup(m => m.GetEnumerator()).Returns(() => candidatesData.GetEnumerator());

            dbContextMock.Setup(x => x.Voters).Returns(voterMockSet.Object);
            dbContextMock.Setup(x => x.Candidates).Returns(candidateMockSet.Object);

            eventAggregatorMock.Setup(x => x.GetEvent<VotedEvent>()).Returns(new VotedEvent());

            voteService = new VoteService(
                voterServiceMock.Object,
                candidateServiceMock.Object,
                dbContextMock.Object,
                eventAggregatorMock.Object,
                messageBoxService.Object);
        }

        [TestMethod]
        public void VoteTest01_ShouldBeCorrect()
        {
            voteService.Vote("Jan Kowalski", "Kazimierz Wielki");

            voterServiceMock.Verify(x => x.VoteByVoter("Jan Kowalski"), Times.Once);
            candidateServiceMock.Verify(x => x.VoteOnCandidate("Kazimierz Wielki"), Times.Once);
            dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
            eventAggregatorMock.Verify(x => x.GetEvent<VotedEvent>(), Times.Once);
        }

        private static IList<Voter> GetSampleVotersData() => new List<Voter>
            {
                new Voter { Id = 1, FullName = "Jan Kowalski", HasVoted = false },
                new Voter { Id = 2, FullName = "Adam Nowak", HasVoted = true}
            };

        private static IList<Candidate> GetSampleCandidatesData() => new List<Candidate>
            {
                new Candidate { Id = 1, FullName = "Kazimierz Wielki", Votes = 1 },
                new Candidate { Id = 2, FullName = "Bolesław Chrobry", Votes = 0 }
            };
    }
}
