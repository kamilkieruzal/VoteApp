namespace VoteApp.Interfaces.Interfaces
{
    public interface IAddService<out T>
    {
        bool TryAdd(string firstName, string surname);
    }
}
