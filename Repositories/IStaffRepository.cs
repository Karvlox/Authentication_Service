using Authentication_Service.Models;

namespace Authentication_Service.Repositories;
public interface IStaffRepository
{
    Task<Staff?> GetByCiAsync(int ci);
    Task<Staff?> GetByIdAsync(Guid id);
    Task AddAsync(Staff staff);
    Task UpdateAsync(Staff staff);
}
