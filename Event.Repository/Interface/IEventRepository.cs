namespace Event.Repository.Interface
{
    using System.Data;
    using System.Threading.Tasks;

    using Event.Core;

    public interface IEventRepository
    {
        Task<bool> AddEvent(EventViewModel objEventViewModel);

        Task<bool> DeleteEvent(int id);

        Task<bool> EditEvent(EventViewModel objEventViewModel);

        Task<DataTable> GetAllEvent(int pageIndex, int pageSize, int userId, int? eventId);

        Task<bool> GetEventByName(string name);
    }
}