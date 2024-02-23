using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO.DuAn;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class DuAnRepository : IDuAnRepo
    {
        private readonly AppDbContext _dbContext;

        public DuAnRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //DuAn methods
        public List<DuAn> GetAllDuAns()
        {
            var duAns = _dbContext.DuAns
                .Where(duAn => duAn.DeletedTime == null)
                .OrderByDescending(duAn => duAn.CreatedTime)
                .Include(duAn => duAn.Leader)
                .ToList();

            return duAns;
        }

        public DuAn GetDuAnById(string id)
        {
            var duAn = _dbContext.DuAns
                .Where(duAn => duAn.DeletedTime == null)
                .Include(duAn => duAn.Leader)
                .FirstOrDefault(c => c.Id == id);

            return duAn;
        }

        public DuAn GetDuAnByName(string projectName)
        {
            return _dbContext.DuAns.FirstOrDefault(d => d.Ten == projectName && d.DeletedTime == null)!;
        }

        public List<DuAnModel> SearchProject(string? ten, string? leaderName, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.DuAns.Where(d => d.DeletedTime == null).AsQueryable();

            if (!string.IsNullOrEmpty(ten))
            {
                query = query.Where(d => d.Ten.Contains(ten));
            }
            if (!string.IsNullOrEmpty(leaderName))
            {
                query = query.Where(d => d.Leader.HoVaTen != null && d.Leader.HoVaTen.Contains(leaderName));
            }
            if (startDate.HasValue)
            {
                query = query.Where(d => d.ThoiGianBatDau >= startDate);
            }
            if (endDate.HasValue)
            {
                query = query.Where(d => d.ThoiGianKetThuc <= endDate);
            }

            // Sorting
            query = query.OrderBy(d => d.Ten);

            var result = query.Select(d => new DuAnModel
            {
                Ten = d.Ten,
                LeaderId = d.LeaderId,
                LeaderName = d.Leader.HoVaTen,
                ThoiGianBatDau = d.ThoiGianBatDau,
                ThoiGianKetThuc = d.ThoiGianKetThuc,
            });

            return result.ToList();

        }

        public int CreateDuAn(string user, DuAn createDuAn)
        {
            var existingDuAn = GetDuAnByName(createDuAn.Ten);
            if (existingDuAn != null)
            {
                return -1;
            }

            createDuAn.CreatedBy = user;
            createDuAn.LastUpdatedBy = user;
            createDuAn.LastUpdatedTime = DateTime.Now;

            _dbContext.DuAns.Add(createDuAn);
            _dbContext.SaveChanges();

            return 1;
        }

        public int UpdateDuAn(string duAnId, string user, DuAn updatedDuAn)
        {
            var existingDuAn = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId 
                                                                  && d.DeletedTime == null);
            if (existingDuAn == null)
            {
                throw new Exception($"DuAn with ID ({duAnId}) not found.");
            }

            if (updatedDuAn.Ten != null)
            {
                existingDuAn.Ten = updatedDuAn.Ten;
                var check = _dbContext.DuAns.SingleOrDefault(x => x.Ten == existingDuAn.Ten && x.DeletedBy == null && x.Id != existingDuAn.Id);
                if (check != null)
                {
                    return 0;
                }
            }

            existingDuAn.LeaderId = updatedDuAn.LeaderId;
            existingDuAn.ThoiGianBatDau = updatedDuAn.ThoiGianBatDau;
            existingDuAn.ThoiGianKetThuc = updatedDuAn.ThoiGianKetThuc;
            existingDuAn.LastUpdatedBy = user;
            existingDuAn.LastUpdatedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        public int DeleteDuAn(string duAnId, string user)
        {
            var duAnToDelete = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId && d.DeletedTime == null);
            if (duAnToDelete == null)
            {
                throw new Exception($"DuAn with ID ({duAnId}) not found.");
            }

            duAnToDelete.DeletedBy = user;
            duAnToDelete.DeletedTime = DateTime.Now;

            //_dbContext.Remove(duAnToDelete);
            return _dbContext.SaveChanges();
        }

        // UserDuAn methods
        public List<UserDuAn> GetAllUsersInDuAn(string duAnId)
        {
            return _dbContext.UserDuAns.Where(x => x.IdDuAn == duAnId && x.DeletedTime == null)
                                                    .OrderByDescending(da => da.CreatedTime)
                                                    .Include(da => da.DuAn)
                                                    .Include(da => da.User)
                                                    .ToList();
        }

        public int AddUserToDuAn(string duAnId, string user, UserDuAn addUserDuAn)
        {
            //var duAn = _dbContext.DuAns.Find(duAnId);

            //if (duAn == null)
            //{
            //    throw new Exception($"DuAn with ID ({duAnId}) not found.");
            //}

            //var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == addUserDuAn.UserId && u.DeletedTime == null);
            //if (existingUser == null)
            //{
            //    throw new Exception($"User with ID {existingUser.Id} not found.");
            //}

            //var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == addUserDuAn.UserId
            //                                                     && x.IdDuAn == duAnId
            //                                                     && x.DeletedTime == null);
            //if (userDuAn == null)
            //{
            //    throw new Exception($"UserDuAn with ID ({addUserDuAn.UserId}) in DuAn '{addUserDuAn.DuAn.Ten}' not found.");
            //}

            addUserDuAn.IdDuAn = duAnId;

            addUserDuAn.CreatedBy = user;
            addUserDuAn.LastUpdatedBy = user;
            addUserDuAn.LastUpdatedTime = DateTime.Now;

            _dbContext.UserDuAns.Add(addUserDuAn);
            return _dbContext.SaveChanges();
        }

        public int UpdateUserInDuAn(string duAnId, string user, UserDuAn updateUserDuAn)
        {
            var duAn = _dbContext.DuAns.Find(duAnId);

            if (duAn == null)
            {
                throw new Exception($"DuAn with ID ({duAnId}) not found.");
            }

            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == updateUserDuAn.UserId 
                                                                 && x.IdDuAn == duAnId 
                                                                 && x.DeletedTime == null);

            if (userDuAn == null)
            {
                throw new Exception($"UserDuAn with ID ({updateUserDuAn.UserId}) in DuAn '{updateUserDuAn.DuAn.Ten}' not found.");
            }

            userDuAn.ViTri = updateUserDuAn.ViTri;

            userDuAn.LastUpdatedBy = user;
            userDuAn.LastUpdatedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        public int DeleteUserFromDuAn(string duAnId, string user, string userId)
        {
            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.IdDuAn == duAnId  
                                                                 && x.UserId == userId 
                                                                 && x.DeletedTime == null);

            if (userDuAn == null)
            {
                throw new Exception($"UserDuAn with ID ({userId}) in DuAn with ID '{duAnId}' not found.");
            }

            var duAn = _dbContext.DuAns.Find(duAnId);

            if (duAn == null)
            {
                throw new Exception($"DuAn with ID ({duAnId}) not found.");
            }

            userDuAn.DeletedBy = user;
            userDuAn.DeletedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }
    }
}
