using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Services
{
    public interface IDashboardService
    {
        /*public void UpdateInternedCount();
        public Dashboard GetDashboard();*/

        public int GetTotalUsersWithStatusTrue();
        public int GetTotalUsersWithStatusTrueAndYear(int year);
       
    }
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
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
