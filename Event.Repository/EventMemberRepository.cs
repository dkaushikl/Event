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

    public class EventMemberRepository : IEventMemberRepository
    {
        private readonly SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities entities = new EventEntities();

        public async Task<bool> AddEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            var objEventMember = new EventMember
            {
                Id = objEventMemberViewModel.Id,
                UserId = objEventMemberViewModel.UserId,
                EventId = objEventMemberViewModel.EventId,
                CreatedDate = DateTime.Now,
                IsActive = objEventMemberViewModel.IsActive
            };

            this.entities.EventMembers.Add(objEventMember);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEventMember(int id)
        {
            var eventDeactive = this.entities.EventMembers.FirstOrDefault(x => x.Id == id);
            if (eventDeactive == null)
            {
                return false;
            }

            this.entities.EventMembers.Remove(eventDeactive);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditEventMember(EventMemberViewModel objEventMemberViewModel)
        {
            var objEventMember = await this.entities.EventMembers.FirstOrDefaultAsync(x => x.Id == objEventMemberViewModel.Id);

            if (objEventMember == null)
            {
                return false;
            }

            objEventMember.UserId = objEventMemberViewModel.UserId;
            objEventMember.EventId = objEventMemberViewModel.EventId;
            objEventMember.CreatedDate = objEventMemberViewModel.CreatedDate;
            objEventMember.IsActive = objEventMemberViewModel.IsActive;

            this.entities.Entry(objEventMember).State = System.Data.Entity.EntityState.Modified;
            await this.entities.SaveChangesAsync();

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
                           this.conn,
                           CommandType.StoredProcedure,
                           "GetAllEventMember",
                           objSqlParameters);
            }
            finally
            {
                this.conn.Close();
            }
        }

        public async Task<bool> CheckEventUserExist(long userId, long eventId)
        {
            var eventExist = await this.entities.EventMembers.AnyAsync(x => x.UserId == userId && x.EventId == eventId);
            return eventExist;
        }
    }
}