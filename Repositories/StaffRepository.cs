using Authentication_Service.Data;
using Authentication_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Service.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Staff?> GetByCiAsync(int ci)
    {
        return await _context.Staffs.FirstOrDefaultAsync(u => u.Ci == ci);
    }

    public async Task<Staff?> GetByIdAsync(Guid id)
    {
        return await _context.Staffs.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddAsync(Staff staff)
    {
        await _context.Staffs.AddAsync(staff);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Staff staff)
    {
        _context.Staffs.Update(staff);
        await _context.SaveChangesAsync();
    }
}