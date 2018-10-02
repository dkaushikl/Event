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

    public class CompanyMemberRepository : ICompanyMemberRepository
    {
        private readonly SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities entities = new EventEntities();

        public async Task<bool> AddCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            var objCompanyMember = new CompanyMember
            {
                Id = objCompanyMemberViewModel.Id,
                UserId = objCompanyMemberViewModel.UserId,
                CompanyId = objCompanyMemberViewModel.CompanyId,
                CreatedDate = DateTime.Now,
                IsActive = objCompanyMemberViewModel.IsActive
            };

            this.entities.CompanyMembers.Add(objCompanyMember);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCompanyMember(int id)
        {
            var companyDeactive = this.entities.CompanyMembers.FirstOrDefault(x => x.Id == id);

            if (companyDeactive == null)
            {
                return false;
            }

            this.entities.CompanyMembers.Remove(companyDeactive);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel)
        {
            var objCompanyMember = await this.entities.CompanyMembers.FirstOrDefaultAsync(x => x.Id == objCompanyMemberViewModel.Id);

            if (objCompanyMember == null)
            {
                return false;
            }

            objCompanyMember.Id = objCompanyMemberViewModel.Id;
            objCompanyMember.CompanyId = objCompanyMemberViewModel.CompanyId;
            objCompanyMember.UserId = objCompanyMemberViewModel.UserId;
            objCompanyMember.CreatedDate = objCompanyMemberViewModel.CreatedDate;
            objCompanyMember.IsActive = objCompanyMemberViewModel.IsActive;

            this.entities.Entry(objCompanyMember).State = System.Data.Entity.EntityState.Modified;
            await this.entities.SaveChangesAsync();

            return true;
        }

        public async Task<DataTable> GetAllCompanyMember(int pageIndex, int pageSize, int userId, int? companyId = 0)
        {
            try
            {
                var objSqlParameters = new SqlParameter[4];
                objSqlParameters[0] = new SqlParameter("@CompanyMemberId", companyId);
                objSqlParameters[1] = new SqlParameter("@UserId", userId);
                objSqlParameters[2] = new SqlParameter("@PageIndex", pageIndex);
                objSqlParameters[3] = new SqlParameter("@PageSize", pageSize);
                return await SqlHelper.ExecuteDataTableAsync(
                           this.conn,
                           CommandType.StoredProcedure,
                           "GetAllCompanyMember",
                           objSqlParameters);
            }
            finally
            {
                this.conn.Close();
            }
        }

        public async Task<bool> CheckUserExist(long companyId, string email)
        {
            var companyExist = await this.entities.CompanyMembers.AnyAsync(x => x.CompanyId == companyId && x.User.Email.ToLower() == email.ToLower());
            return companyExist;
        }
    }
}