﻿namespace Event.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using Event.Core;
    using Event.Data;
    using Event.Repository.Interface;

    public class CompanyRepository : ICompanyRepository
    {
        private readonly SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        private readonly EventEntities entities = new EventEntities();

        public async Task<bool> AddCompany(CompanyViewModel objCompanyViewModel)
        {
            var objCompany = new Company
            {
                Id = objCompanyViewModel.Id,
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

            if (objCompany == null)
            {
                return false;
            }

            objCompany.Id = objCompanyViewModel.Id;
            objCompany.Name = objCompanyViewModel.Name;
            objCompany.Address = objCompanyViewModel.Address;
            objCompany.City = objCompanyViewModel.City;
            objCompany.Country = objCompanyViewModel.Country;
            objCompany.Email = objCompanyViewModel.Email;
            objCompany.MobileNo = objCompanyViewModel.MobileNo;
            objCompany.State = objCompanyViewModel.State;
            objCompany.CreatedBy = objCompanyViewModel.CreatedBy;
            objCompany.CreatedDate = objCompanyViewModel.CreatedDate;
            objCompany.IsActive = objCompanyViewModel.IsActive;

            this.entities.Entry(objCompany).State = EntityState.Modified;
            await this.entities.SaveChangesAsync();

            return true;
        }

        public async Task<List<CompanyViewModel>> GetAllCompany() =>
            await (from objCompany in this.entities.Companies
                   select new CompanyViewModel
                   {
                       Id = objCompany.Id,
                       Name = objCompany.Name,
                       Address = objCompany.Address,
                       City = objCompany.City,
                       Country = objCompany.Country,
                       Email = objCompany.Email,
                       MobileNo = objCompany.MobileNo,
                       State = objCompany.State,
                       CreatedBy = objCompany.CreatedBy,
                       CreatedDate = objCompany.CreatedDate,
                       IsActive = objCompany.IsActive
                   }).ToListAsync();

        public async Task<DataTable> GetAllCompanyUsingSp()
        {
            try
            {
                return await SqlHelper.ExecuteDataTableAsync(this.conn, CommandType.StoredProcedure, "GetCompany");
            }
            finally
            {
                this.conn.Close();
            }
        }

        public async Task<CompanyViewModel> GetCompanyById(int id)
        {
            var objCompany = await this.entities.Companies.FirstOrDefaultAsync(x => x.Id == id);

            return objCompany == null
                       ? null
                       : new CompanyViewModel
                       {
                           Id = objCompany.Id,
                           Name = objCompany.Name,
                           Address = objCompany.Address,
                           City = objCompany.City,
                           Country = objCompany.Country,
                           Email = objCompany.Email,
                           MobileNo = objCompany.MobileNo,
                           State = objCompany.State,
                           CreatedBy = objCompany.CreatedBy,
                           CreatedDate = objCompany.CreatedDate,
                           IsActive = objCompany.IsActive
                       };
        }

        public async Task<CompanyViewModel> GetCompanyByName(string name)
        {
            var objCompany =
                await this.entities.Companies.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());

            return objCompany == null
                       ? null
                       : new CompanyViewModel
                       {
                           Id = objCompany.Id,
                           Name = objCompany.Name,
                           Address = objCompany.Address,
                           City = objCompany.City,
                           Country = objCompany.Country,
                           Email = objCompany.Email,
                           MobileNo = objCompany.MobileNo,
                           State = objCompany.State,
                           CreatedBy = objCompany.CreatedBy,
                           CreatedDate = objCompany.CreatedDate,
                           IsActive = objCompany.IsActive
                       };
        }
    }
}