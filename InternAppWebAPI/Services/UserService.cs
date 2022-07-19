using InternAppWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternAppWebAPI.Services
{
    public class UserService
    {
        public List<RosenUser> users = null;

        InternAppDBContext _dbContext = new InternAppDBContext();

        public InternAppDBContext getDbContext()
        {
            return this._dbContext;
        }

        public async Task getUsers()
        {
            try
            {
                users = await _dbContext.RosenUsers.ToListAsync();
            }
            catch (Exception error)
            {
                users = null;
            }
        }
        public async Task<List<RosenUser>> getUsersByTitleId(int titleId)
        {
            List<RosenUser> results = await _dbContext.RosenUsers.Where(user => user.UserTitleId == titleId).ToListAsync();
            return results;
        }

        public async Task<RosenUser> checkEmailExists(string email)
        {
            RosenUser user = await _dbContext.RosenUsers.Where(user => user.UserEmail == email).SingleOrDefaultAsync();
            return user;
        }

    }
}
