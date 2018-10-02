namespace Event.Repository.Interface
{
    using System.Data;
    using System.Threading.Tasks;

    using Event.Core;

    public interface ICompanyRepository
    {
        Task<bool> AddCompany(CompanyViewModel objCompanyViewModel);

        Task<bool> DeleteCompany(int id);

        Task<bool> EditCompany(CompanyViewModel objCompanyViewModel);

        Task<DataTable> GetAllCompany(int pageIndex, int pageSize, int userId, int? companyId);

        Task<bool> GetCompanyByName(string name, long companyId);
    }
}