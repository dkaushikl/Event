namespace Event.Repository.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Event.Core;

    public interface ICompanyRepository
    {
        Task<bool> AddCompany(CompanyViewModel objCompanyViewModel);

        Task<bool> DeleteCompany(int id);

        Task<bool> EditCompany(CompanyViewModel objCompanyViewModel);

        Task<List<CompanyViewModel>> GetAllCompany();

        Task<CompanyViewModel> GetCompanyById(int id);

        Task<CompanyViewModel> GetCompanyByName(string name);
    }
}