using System.ComponentModel.DataAnnotations;

namespace VoteApp.Interfaces.Models
{
    public abstract class PersonEntity
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}
