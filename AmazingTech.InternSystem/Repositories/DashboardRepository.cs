using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface IDashboardRepository
    {
       /* public Dashboard GetDashboard();
        public void UpdateDashboard(Dashboard dashboard);*/
        /*public int GetTotalUsersWithStatusTrue();*/

        public int GetTotalUsersWithStatusTrueAnhTuan();
        public int GetTotalUsersWithStatusTrueAndYear(int year);
        

    }
    public class DashboardRepository : IDashboardRepository
    {

        public int GetTotalUsersWithStatusTrueAnhTuan()
        {
            using (var context = new AppDbContext())
            {
                return context.InternInfos.Count(info => info.Status == "true");
            }
        }
        public int GetTotalUsersWithStatusTrueAndYear(int year)
        {
            using (var context = new AppDbContext())
            {
                return context.InternInfos.Count(info => info.Status == "true" && info.CreatedTime.HasValue && info.CreatedTime.Value.Year == year);
            }
        }

        
    }
}
