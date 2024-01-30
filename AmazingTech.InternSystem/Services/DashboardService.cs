using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Services
{
    public interface IDashboardService
    {
        public int CountInternSendCVInAYear(int year);
        public int CountInternSendCVInPreciousOfYear(int year, int precious);
        public int CountInternInterviewedInAYear(int year);
        public int CountInternInterviewedInPreciousOfYear(int year, int precious);
        /*public void UpdateInternedCount();
        public Dashboard GetDashboard();*/

        public int GetTotalUsersWithStatusTrue();
        public int GetTotalUsersWithStatusTrueAndYear(int year);
    }
    public class DashboardService : IDashboardService
    {
        private readonly IInternInfoRepo _internInfoRepo;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IInternInfoRepo internInfoRepo , IUserRepository userRepository, UserManager<User> userManager,IDashboardRepository dashboardRepository)
        {
            _internInfoRepo = internInfoRepo;
            _userRepository = userRepository;
            _userManager = userManager;
            _dashboardRepository = dashboardRepository;
        }
        public int CountInternSendCVInAYear(int year)
        {
            if(year < 0)
            {
                throw new BadHttpRequestException("Year can be negative");
            }
            return _internInfoRepo.GetInternSendCVInAYear(year);
        }
        public int CountInternSendCVInPreciousOfYear(int year,int precious)
        {
            if(year < 0 || precious<0)
            {
                throw new BadHttpRequestException("Year or Precious can be negative");
            }
            if (precious >= 5)
            {
                throw new BadHttpRequestException("Làm đéo gì có quý lớn hơn 4 mà search chi vậy ");
            }
            return _internInfoRepo.GetInternSendCVInPrecious(year, precious);
        }
        public int CountInternInterviewedInAYear(int year)
        {
            if(year < 0)
            {
                throw new BadHttpRequestException("Year can be negative");
            }
            var listUser = _userRepository.GetUserHavingInterviewScheduleAndStatusDoneInAYear(year);
            var listIntern = listUser.Where(listUser => _userManager.IsInRoleAsync(listUser,Roles.INTERN).Result).ToList().Count;
            return listIntern; 
        }
        public int CountInternInterviewedInPreciousOfYear(int year, int precious)
        {
            if (year < 0 || precious < 0)
            {
                throw new BadHttpRequestException("Year or Precious can be negative");
            }
            if (precious >= 5)
            {
                throw new BadHttpRequestException("Làm đéo gì có quý lớn hơn 4 mà search chi vậy ");
            }
            var listUser = _userRepository.GetUsersHavingInterviewScheduleAndStatusDoneInAQuarter(year,precious);
            var listIntern = listUser.Where(listUser => _userManager.IsInRoleAsync(listUser, Roles.INTERN).Result).ToList().Count;
            return listIntern;
        }



      

        public int GetTotalUsersWithStatusTrue()
        {
            return _dashboardRepository.GetTotalUsersWithStatusTrueAnhTuan();
        }
        public int GetTotalUsersWithStatusTrueAndYear(int year)
        {
            return _dashboardRepository.GetTotalUsersWithStatusTrueAndYear(year);
        }
    }
}
