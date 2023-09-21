using VoteApp.Interfaces.Models;

namespace VoteApp.Models
{
    public class Voter : PersonEntity
    {
        public bool HasVoted { get; set; }
    }
}
