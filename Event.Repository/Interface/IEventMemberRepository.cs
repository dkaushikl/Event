namespace Event.Repository.Interface
{
    using System.Data;
    using System.Threading.Tasks;

    using Event.Core;

    public interface IEventMemberRepository
    {
        Task<bool> AddEventMember(EventMemberViewModel objEventMemberViewModel);

        Task<bool> DeleteEventMember(int id);

        Task<bool> EditEventMember(EventMemberViewModel objEventMemberViewModel);

        Task<DataTable> GetAllEventMember(int pageIndex, int pageSize, int userId, int? eventMemberId);

        Task<bool> CheckEventUserExist(long userId, long eventId);
    }
}