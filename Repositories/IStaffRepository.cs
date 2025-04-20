using Authentication_Service.Models;

namespace Authentication_Service.Repositories;
public interface IStaffRepository
{
    Task<Staff?> GetByCiAsync(int ci);
    Task AddAsync(Staff staff);
}
