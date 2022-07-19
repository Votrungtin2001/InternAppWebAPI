using InternAppWebAPI.Models;
using InternAppWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternAppWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        InternAppDBContext _dbContext; 

        public UserController(UserService userService)
        {
            _userService = userService;
            _dbContext = _userService.getDbContext();
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult> getUsers()
        {
            if (_userService.users != null) return Ok(_userService.users);

            try
            {
                await _userService.getUsers();
                return Ok(_userService.users);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }

        }

        // GET: api/user/{userId}
        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult> getUser(int userId)
        {
            try
            {
                var rosenUser = await _dbContext.RosenUsers.Where(user => user.UserId == userId)
                 .Join(
                 this._dbContext.Titles,
                 user => user.UserTitleId,
                 title => title.TitleId,
                 (user, title) => new
                 {
                     newUser = new
                     {
                         UserId = user.UserId,
                         UserFirstName = user.UserFirstName,
                         UserLastName = user.UserLastName,
                         UserDOB = user.UserDob,
                         UserGender = user.UserGender,
                         UserCompany = user.UserCompany,
                         UserTitleId = title.TitleId,
                         UserTitleName = title.TitleName,
                         UserEmail = user.UserEmail,
                         UserImage = user.UserImage,
                         UserCreatedDate = user.UserCreatedDate,
                         status = "Active",
                     },
                 }
                 ).FirstAsync();

                if(rosenUser == null) return StatusCode(404, "User not found");

                return Ok(rosenUser.newUser);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }

        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult> addNewUser(RosenUser newUser)
        {
            if (newUser == null)
            {
                return StatusCode(405, "Invalid input");
            }

            try
            {
                long createdDate = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                newUser.UserCreatedDate = createdDate;

                this._dbContext.RosenUsers.Add(newUser);
                await this._dbContext.SaveChangesAsync();

                return this.CreatedAtAction("getUser", new { userId = newUser.UserId }, newUser);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }

        }

        // PATCH: api/user
        [HttpPatch]
        public async Task<ActionResult> editUser(RosenUser user)
        {
            if (user == null)
            {
                return StatusCode(405, "Invalid input");
            }

            try
            {
                this._dbContext.Attach(user);
                this._dbContext.Entry(user).State = EntityState.Modified;
                await this._dbContext.SaveChangesAsync();
                return this.CreatedAtAction("getUser", new { userId = user.UserId }, user);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }
        }

        // DELETE: api/user/{userId}
        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult> deleteUser(int userId)
        {
            try
            {
                RosenUser user = await this._dbContext.RosenUsers.Where(u => u.UserId == userId).SingleOrDefaultAsync<RosenUser>();
                if (user == null) return StatusCode(404, "User not found");

                this._dbContext.Remove(user);
                await this._dbContext.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }
        }

        // GET: api/bytitle/{titleId}
        [HttpGet]
        [Route("byTitle/{titleId}")]
        public async Task<ActionResult> getUsersGroupByTitleId(int titleId)
        {
            try
            {
                List<RosenUser> results = await _userService.getUsersByTitleId(titleId);
                return Ok(results);
            }
            catch (Exception error)
            {
                return StatusCode(404, error.Message);
            }
          
        }

        // POST: api/bytitlewithsearch
        [HttpPost]
        [Route("byTitleWithSearch")]
        public async Task<ActionResult> getUsersGroupByTitleIdWithSearch([FromBody] DataModelSearchUser model)
        {
            try
            {
                List<RosenUser> resultsBeforeSearch = await _userService.getUsersByTitleId(model.titleId);
                List<RosenUser> results = new List<RosenUser>();

                foreach (RosenUser user in resultsBeforeSearch)
                {
                    string UserFullName = user.UserLastName + " " + user.UserFirstName;
                    if (UserFullName.ToLower().Contains(model.searchText.ToLower()) ||
                        user.UserId.ToString() == model.searchText ||
                        user.UserGender.ToString() == model.searchText ||
                        user.UserCompany.ToLower() == model.searchText ||
                        user.UserEmail.ToLower().Contains(model.searchText.ToLower())
                        )

                        results.Add(user);
                }

                return Ok(results);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }

        }

        // GET: api/user/email/{email}
        [HttpGet]
        [Route("email/{email}")]

        public async Task<ActionResult> checkEmailExists(string email)
        {
            try
            {
                RosenUser user = await this._userService.checkEmailExists(email);
                if (user == null) return Ok(false);
                return Ok(true);
            }
            catch (Exception error)
            {
                return StatusCode(400, error.Message);
            }

        }


    }
}
