using VoteApp.Models;

namespace VoteApp.Interfaces.Interfaces
{
    public interface ICandidateService : IAddService<Candidate>
    {
        void VoteOnCandidate(string fullName);
    }
}
