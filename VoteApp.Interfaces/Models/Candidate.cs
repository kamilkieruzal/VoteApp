using VoteApp.Interfaces.Models;

namespace VoteApp.Models
{
    public class Candidate : PersonEntity
    {
        public uint Votes { get; set; }
    }
}
