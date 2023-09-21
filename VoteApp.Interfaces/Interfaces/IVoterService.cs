using VoteApp.Models;

namespace VoteApp.Interfaces.Interfaces
{
    public interface IVoterService : IAddService<Voter>
    {
        void VoteByVoter(string fullName);
    }
}
