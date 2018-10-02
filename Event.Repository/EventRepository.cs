﻿namespace Event.Repository
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

    public class EventRepository : IEventRepository
    {
        private readonly SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities entities = new EventEntities();

        public async Task<bool> AddEvent(EventViewModel objEventViewModel)
        {
            var objEvent = new Event
            {
                Id = objEventViewModel.Id,
                Name = objEventViewModel.Name,
                CompanyId = objEventViewModel.CompanyId,
                Description = objEventViewModel.Description,
                EndDate = objEventViewModel.EndDate,
                EndTime = objEventViewModel.EndTime,
                StartDate = objEventViewModel.StartDate,
                StartTime = objEventViewModel.StartTime,
                Vanue = objEventViewModel.Vanue,
                CreatedBy = objEventViewModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = objEventViewModel.IsActive
            };

            this.entities.Events.Add(objEvent);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEvent(int id)
        {
            var eventDeactive = this.entities.Events.FirstOrDefault(x => x.Id == id);
            if (eventDeactive == null)
            {
                return false;
            }

            this.entities.Events.Remove(eventDeactive);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditEvent(EventViewModel objEventViewModel)
        {
            var objEvent = await this.entities.Events.FirstOrDefaultAsync(x => x.Id == objEventViewModel.Id);

            if (objEvent == null)
            {
                return false;
            }

            objEvent.Id = objEventViewModel.Id;
            objEvent.Name = objEventViewModel.Name;
            objEvent.CompanyId = objEventViewModel.CompanyId;
            objEvent.Description = objEventViewModel.Description;
            objEvent.EndDate = objEventViewModel.EndDate;
            objEvent.EndTime = objEventViewModel.EndTime;
            objEvent.StartDate = objEventViewModel.StartDate;
            objEvent.StartTime = objEventViewModel.StartTime;
            objEvent.Vanue = objEventViewModel.Vanue;
            objEvent.CreatedBy = objEventViewModel.CreatedBy;
            objEvent.CreatedDate = objEventViewModel.CreatedDate;
            objEvent.IsActive = objEventViewModel.IsActive;

            this.entities.Entry(objEvent).State = System.Data.Entity.EntityState.Modified;
            await this.entities.SaveChangesAsync();

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
                           this.conn,
                           CommandType.StoredProcedure,
                           "GetAllEvent",
                           objSqlParameters);
            }
            finally
            {
                this.conn.Close();
            }
        }

        public async Task<bool> GetEventByName(string eventName)
        {
            var eventExist = await this.entities.Events.AnyAsync(x => x.Name.ToLower() == eventName.ToLower());
            return eventExist;
        }
    }
}