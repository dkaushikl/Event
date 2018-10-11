namespace Event.Repository
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Event.Core;
    using Event.Data;
    using Event.Repository.Interface;

    using EntityState = System.Data.Entity.EntityState;

    public class EventMemberRepository : IEventMemberRepository
    {
        private readonly SqlConnection _conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities _entities = new EventEntities();

        public async Task<bool> AddEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            var objEventMember = new EventMember
                                     {
                                         UserId = objEventMemberViewModel.UserId,
                                         EventId = objEventMemberViewModel.EventId,
                                         CreatedDate = DateTime.Now,
                                         IsActive = objEventMemberViewModel.IsActive
                                     };

            this._entities.EventMembers.Add(objEventMember);
            await this._entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckEventUserExist(long userId, long eventId)
        {
            var eventExist =
                await this._entities.EventMembers.AnyAsync(x => x.UserId == userId && x.EventId == eventId);
            return eventExist;
        }

        public async Task<bool> DeleteEventMember(int id)
        {
            var eventDeactive = this._entities.EventMembers.FirstOrDefault(x => x.Id == id);
            if (eventDeactive == null) return false;

            this._entities.EventMembers.Remove(eventDeactive);
            await this._entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            var objEventMember =
                await this._entities.EventMembers.FirstOrDefaultAsync(x => x.Id == objEventMemberViewModel.Id);

            if (objEventMember == null) return false;

            objEventMember.UserId = objEventMemberViewModel.UserId;
            objEventMember.EventId = objEventMemberViewModel.EventId;
            objEventMember.CreatedDate = objEventMemberViewModel.CreatedDate;
            objEventMember.IsActive = objEventMemberViewModel.IsActive;

            this._entities.Entry(objEventMember).State = EntityState.Modified;
            await this._entities.SaveChangesAsync();

            return true;
        }

        public async Task<DataTable> GetAllEventMember(int pageIndex, int pageSize, int userId, int? eventId = 0)
        {
            try
            {
                var objSqlParameters = new SqlParameter[4];
                objSqlParameters[0] = new SqlParameter("@EventMemberId", eventId);
                objSqlParameters[1] = new SqlParameter("@UserId", userId);
                objSqlParameters[2] = new SqlParameter("@PageIndex", pageIndex);
                objSqlParameters[3] = new SqlParameter("@PageSize", pageSize);
                return await SqlHelper.ExecuteDataTableAsync(
                           this._conn,
                           CommandType.StoredProcedure,
                           "GetAllEventMember",
                           objSqlParameters);
            }
            finally
            {
                this._conn.Close();
            }
        }
    }
}