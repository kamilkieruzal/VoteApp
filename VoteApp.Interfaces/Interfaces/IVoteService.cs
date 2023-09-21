namespace VoteApp.Interfaces.Interfaces
{
    public interface IVoteService
    {
        void Vote(string voterFullName, string candidateFullName);
    }
}
