using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Data.Enum;
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
            return _dbContext.DuAns
                .Where(duAn => duAn.DeletedTime == null)
                .OrderByDescending(duAn => duAn.CreatedTime)
                .Include(duAn => duAn.Leader)
                .ToList();
        }

        public DuAn? GetDuAnById(string id)
        {
            return _dbContext.DuAns
                .Where(duAn => duAn.DeletedTime == null)
                .Include(duAn => duAn.Leader)
                .FirstOrDefault(c => c.Id == id);
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
                throw new Exception("DuAn with the same name already exists.");
            }

            var existingLeader = _dbContext.Users.SingleOrDefault(d => d.Id == createDuAn.LeaderId && d.DeletedTime == null);
            if (existingLeader == null)
            {
                throw new Exception($"Leader with ID ({createDuAn.LeaderId}) not found.");
            }

            createDuAn.CreatedBy = user;
            createDuAn.LastUpdatedBy = user;
            createDuAn.LastUpdatedTime = DateTime.Now;

            _dbContext.DuAns.Add(createDuAn);
            return _dbContext.SaveChanges();
        }

        public int UpdateDuAn(string duAnId, string user, DuAn updatedDuAn)
        {
            var existingLeader = _dbContext.Users.SingleOrDefault(d => d.Id == updatedDuAn.LeaderId && d.DeletedTime == null);
            if (existingLeader == null)
            {
                throw new Exception($"Leader with ID ({updatedDuAn.LeaderId}) not found.");
            }

            var existingDuAn = _dbContext.DuAns.SingleOrDefault(d => d.Id == duAnId && d.DeletedTime == null);
            if (existingDuAn == null)
            {
                throw new Exception($"DuAn with ID ({duAnId}) not found.");
            }

            //if (updatedDuAn.Ten != null)
            //{
            //    existingDuAn.Ten = updatedDuAn.Ten;
            //    var check = _dbContext.DuAns.SingleOrDefault(x => x.Ten == existingDuAn.Ten && x.DeletedBy == null && x.Id != existingDuAn.Id);
            //    if (check != null)
            //    {
            //        return 0;
            //    }
            //}

            existingDuAn.Ten = updatedDuAn.Ten;
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
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == addUserDuAn.UserId && u.DeletedTime == null);
            if (existingUser == null)
            {
                throw new Exception($"User with ID ({addUserDuAn.UserId}) not found.");
            }

            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == addUserDuAn.UserId
                                                                 && x.IdDuAn == duAnId
                                                                 && x.DeletedTime == null);
            if (userDuAn != null)
            {
                throw new Exception($"User with ID ({addUserDuAn.UserId}) has already existed in this DuAn.");
            }

            var shedule = _dbContext.LichPhongVans.FirstOrDefault(i => i.IdNguoiDuocPhongVan == addUserDuAn.UserId && i.DeletedTime == null);
            if (shedule == null || shedule.KetQua == Result.Failed)
            {
                 throw new Exception("This user has not passed the interview and cannot be added to the project.");
            }

            addUserDuAn.IdDuAn = duAnId;
            addUserDuAn.CreatedBy = user;
            addUserDuAn.LastUpdatedBy = user;
            addUserDuAn.LastUpdatedTime = DateTime.Now;

            _dbContext.UserDuAns.Add(addUserDuAn);
            return _dbContext.SaveChanges();
        }

        public int UpdateUserInDuAn(string duAnId, string user, UserDuAn updateUserDuAn)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == updateUserDuAn.UserId && u.DeletedTime == null);
            if (existingUser == null)
            {
                throw new Exception($"User with ID ({updateUserDuAn.UserId}) not found.");
            }

            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == updateUserDuAn.UserId
                                                                 && x.IdDuAn == duAnId
                                                                 && x.DeletedTime == null);
            if (userDuAn == null)
            {
                throw new Exception($"User with ID ({updateUserDuAn.UserId}) does not exist in this DuAn.");
            }

            userDuAn.ViTri = updateUserDuAn.ViTri;
            userDuAn.LastUpdatedBy = user;
            userDuAn.LastUpdatedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }

        public int DeleteUserFromDuAn(string duAnId, string user, string userId)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Id == userId && u.DeletedTime == null);
            if (existingUser == null)
            {
                throw new Exception($"User with ID ({userId}) not found.");
            }

            var userDuAn = _dbContext.UserDuAns.FirstOrDefault(x => x.UserId == userId
                                                                 && x.IdDuAn == duAnId
                                                                 && x.DeletedTime == null);
            if (userDuAn == null)
            {
                throw new Exception($"User with ID ({userId}) does not exist in this DuAn.");
            }

            userDuAn.DeletedBy = user;
            userDuAn.DeletedTime = DateTime.Now;

            return _dbContext.SaveChanges();
        }
    }
}
