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

    public class EventRepository : IEventRepository
    {
        private readonly SqlConnection _conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities _entities = new EventEntities();

        public async Task<bool> AddEvent(EventViewModel objEventViewModel)
        {
            var objEvent = new Event
                               {
                                   Name = objEventViewModel.Name,
                                   CompanyId = objEventViewModel.CompanyId,
                                   Description = objEventViewModel.Description,
                                   Vanue = objEventViewModel.Vanue,
                                   StartDate = Convert.ToDateTime(objEventViewModel.StartDate),
                                   StartTime = TimeSpan.Parse(objEventViewModel.StartTime),
                                   EndDate = Convert.ToDateTime(objEventViewModel.EndDate),
                                   EndTime = TimeSpan.Parse(objEventViewModel.EndTime),
                                   CreatedBy = objEventViewModel.CreatedBy,
                                   CreatedDate = DateTime.Now,
                                   IsActive = objEventViewModel.IsActive
                               };
            this._entities.Events.Add(objEvent);
            await this._entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEvent(int id)
        {
            var eventDeactive = this._entities.Events.FirstOrDefault(x => x.Id == id);
            if (eventDeactive == null) return false;

            this._entities.Events.Remove(eventDeactive);
            await this._entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditEvent(EventViewModel objEventViewModel)
        {
            var objEvent = await this._entities.Events.FirstOrDefaultAsync(x => x.Id == objEventViewModel.Id);

            if (objEvent == null) return false;

            objEvent.Name = objEventViewModel.Name;
            objEvent.CompanyId = objEventViewModel.CompanyId;
            objEvent.Description = objEventViewModel.Description;
            objEvent.Vanue = objEventViewModel.Vanue;
            objEvent.StartDate = Convert.ToDateTime(objEventViewModel.StartDate);
            objEvent.StartTime = TimeSpan.Parse(objEventViewModel.StartTime);
            objEvent.EndDate = Convert.ToDateTime(objEventViewModel.EndDate);
            objEvent.EndTime = TimeSpan.Parse(objEventViewModel.EndTime);
            objEvent.CreatedBy = objEventViewModel.CreatedBy;
            objEvent.IsActive = objEventViewModel.IsActive;

            this._entities.Entry(objEvent).State = EntityState.Modified;
            await this._entities.SaveChangesAsync();

            return true;
        }

        public async Task<DataTable> GetAllEvent(int pageIndex, int pageSize, int userId, int? eventId = 0)
        {
            try
            {
                var objSqlParameters = new SqlParameter[4];
                objSqlParameters[0] = new SqlParameter("@EventId", eventId);
                objSqlParameters[1] = new SqlParameter("@UserId", userId);
                objSqlParameters[2] = new SqlParameter("@PageIndex", pageIndex);
                objSqlParameters[3] = new SqlParameter("@PageSize", pageSize);
                return await SqlHelper.ExecuteDataTableAsync(
                           this._conn,
                           CommandType.StoredProcedure,
                           "GetAllEvent",
                           objSqlParameters);
            }
            finally
            {
                this._conn.Close();
            }
        }

        public async Task<bool> GetEventByName(string eventName, long eventId)
        {
            var eventExist =
                await this._entities.Events.AnyAsync(x => x.Name.ToLower() == eventName.ToLower() && x.Id != eventId);
            return eventExist;
        }
    }
}