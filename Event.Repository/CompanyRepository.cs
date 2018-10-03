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

    public class CompanyRepository : ICompanyRepository
    {
        private readonly SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities entities = new EventEntities();

        public async Task<bool> AddCompany(CompanyViewModel objCompanyViewModel)
        {
            var objCompany = new Company
            {
                Name = objCompanyViewModel.Name,
                Address = objCompanyViewModel.Address,
                City = objCompanyViewModel.City,
                Country = objCompanyViewModel.Country,
                Email = objCompanyViewModel.Email,
                MobileNo = objCompanyViewModel.MobileNo,
                State = objCompanyViewModel.State,
                CreatedBy = objCompanyViewModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = objCompanyViewModel.IsActive
            };

            this.entities.Companies.Add(objCompany);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCompany(int id)
        {
            var companyDeactive = this.entities.Companies.FirstOrDefault(x => x.Id == id);
            if (companyDeactive == null)
            {
                return false;
            }

            this.entities.Companies.Remove(companyDeactive);
            await this.entities.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditCompany(CompanyViewModel objCompanyViewModel)
        {
            var objCompany = await this.entities.Companies.FirstOrDefaultAsync(x => x.Id == objCompanyViewModel.Id);

            if (objCompany?.CreatedBy != objCompanyViewModel.CreatedBy)
            {
                return false;
            }

            objCompany.Name = objCompanyViewModel.Name;
            objCompany.Address = objCompanyViewModel.Address;
            objCompany.City = objCompanyViewModel.City;
            objCompany.Country = objCompanyViewModel.Country;
            objCompany.Email = objCompanyViewModel.Email;
            objCompany.MobileNo = objCompanyViewModel.MobileNo;
            objCompany.State = objCompanyViewModel.State;
            objCompany.IsActive = objCompanyViewModel.IsActive;

            this.entities.Entry(objCompany).State = EntityState.Modified;
            await this.entities.SaveChangesAsync();

            return true;
        }

        public async Task<DataTable> GetAllCompany(int pageIndex, int pageSize, int userId, int? companyId = 0)
        {
            try
            {
                var objSqlParameters = new SqlParameter[4];
                objSqlParameters[0] = new SqlParameter("@CompanyId", companyId);
                objSqlParameters[1] = new SqlParameter("@UserId", userId);
                objSqlParameters[2] = new SqlParameter("@PageIndex", pageIndex);
                objSqlParameters[3] = new SqlParameter("@PageSize", pageSize);
                return await SqlHelper.ExecuteDataTableAsync(
                           this.conn,
                           CommandType.StoredProcedure,
                           "GetAllCompany",
                           objSqlParameters);
            }
            finally
            {
                this.conn.Close();
            }
        }

        public async Task<bool> GetCompanyByName(string companyName, long companyId)
        {
            var companyExist =
                await this.entities.Companies.AnyAsync(
                    x => x.Id != companyId && x.Name.ToLower() == companyName.ToLower());
            return companyExist;
        }
    }
}