using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories
{
    public class UserRepository
    {
        private readonly List<User> _users = [];

        public List<User> GetAll() => _users;

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public void Add(User user)
        {
            user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
        }

        public bool Update(User user)
        {
            var existing = GetById(user.Id);
            if (existing == null) return false;
            existing.Name = user.Name;
            existing.Email = user.Email;
            return true;
        }

        public bool Delete(int id)
        {
            var user = GetById(id);
            if (user == null) return false;
            _users.Remove(user);
            return true;
        }
    }
}
