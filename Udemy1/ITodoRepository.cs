namespace Udemy1
{
    internal interface ITodoRepository
    {
        void Delete(int id);
        Task Get(int id);
        void Save(Task task);
        void Update(Task task);
    }
}