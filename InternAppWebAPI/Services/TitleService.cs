using InternAppWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternAppWebAPI.Services
{
    public class TitleService
    {
        public List<Title> titles = null;
        InternAppDBContext _dbContext = new InternAppDBContext();


        public async Task<List<Title>> getTitles()
        {
            titles = await _dbContext.Titles.ToListAsync();
            return titles;
        }

    }
}
