namespace Event.Repository.Interface
{
    using System.Data;
    using System.Threading.Tasks;

    using Event.Core;

    public interface ICompanyMemberRepository
    {
        Task<bool> AddCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel);

        Task<bool> CheckUserExist(long companyId, string email);

        Task<bool> DeleteCompanyMember(int id);

        Task<bool> EditCompanyMember(CompanyMemberViewModel objCompanyMemberViewModel);

        Task<DataTable> GetAllCompanyMember(int companyId);
    }
}