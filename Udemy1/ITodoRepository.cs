namespace Udemy1
{
    internal interface ITodoRepository
    {
        IReadOnlyCollection<Task> Tasks { get; }
        void Delete(int id);
        Task? Get(int id);
        void Save(Task task);
        void Update(Task task);

        void ViewAll();
    }
}