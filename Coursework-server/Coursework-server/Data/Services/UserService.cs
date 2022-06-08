using Coursework_server.Data.Models;
using Coursework_server.Data.ViewModels;
using Coursework_server.Helpers;
using Coursework_server.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Coursework_server.Data.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext context)
        {
            _db = context;
        }
        
        public User? GetUserByEmailAndPassword(string email, string password) => 
            _db.Users.FirstOrDefault(a => a.Email == email && a.Password == HashHelper.ComputeSha3Hash(password));

        public User? GetUserByEmail(string email) => _db.Users.FirstOrDefault(a => a.Email == email);

        public List<User> GetUsers() => _db.Users.OrderBy(u => u.Name).ToList();

        public int GetUsersCount() => _db.Users.Count();

        public User? GetUserById(Guid id) => _db.Users.FirstOrDefault(u => u.Id == id);
        public User? GetUserById(string id) => _db.Users.FirstOrDefault(u => u.Id == Guid.Parse(id));

        public List<User> SearchUsersByName(string name) => 
            _db.Users.Where(u => EF.Functions.Like(u.Name, $"%{name}%")).ToList();

        public void UpdateUser(User user)
        {
            _db.Users.Update(user);

            _db.SaveChanges();
        }

        public void RemoveUser(User user) 
        {
            _db.Users.Remove(user);

            _db.SaveChanges();
        }

        public List<UserVm> GetPaginatedUsers(UsersPageParams pageParams) => _db.Users
            .OrderBy(u => u.Name)
            .Skip((pageParams.Page - 1) * pageParams.Size)
            .Take(pageParams.Size)
            .Select(u => ConvertHelper.ToUserVm(u))
            .ToList();

        public void BlockUser(Guid id)
        {
            var user = GetUserById(id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            user.UserState = UserState.Blocked;

            UpdateUser(user);
        }

        public void UnBlockUser(Guid id)
        {
            var user = GetUserById(id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            user.UserState = UserState.Active;

            UpdateUser(user);
        }

        public void RemoveUser(Guid id)
        {
            var user = GetUserById(id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            RemoveUser(user);
        }

        public void AddAdmin(Guid id)
        {
            var user = GetUserById(id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            user.UserRole = UserRole.Admin;

            UpdateUser(user);
        }

        public UserVm GetUserVm(string id)
        {
            var user = GetUserById(id);

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            return ConvertHelper.ToUserVm(user);
        }

        public List<UserVm> GetFoundUserVMs(string query)
        {
            var users = SearchUsersByName(query);
            var userVMs = users.Select(ConvertHelper.ToUserVm).ToList();

            return userVMs;
        }
    }
}
