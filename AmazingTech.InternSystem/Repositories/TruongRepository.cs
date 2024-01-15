using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace AmazingTech.InternSystem.Repositories
{
    public class TruongRepository : ITruongRepository
    {
        private DbSet<TruongHoc> _DbSet;

        public TruongRepository() { }

        public int AddTruong(TruongHoc truong)
        {
            using (var context = new AppDbContext()) {
                context.Set<TruongHoc>().Add(truong);
                return context.SaveChanges();
            }
        }

        public int DeleteTruong(string id)
        {
            throw new NotImplementedException();
        }

        public List<TruongHoc> GetAllTruongs()
        {
            using (var context = new AppDbContext())
            {
                var truongs = context.Set<TruongHoc>().ToList();
                Console.WriteLine("Truong: " + truongs);
                return truongs;
            }
        }

        public TruongHoc GetTruong(string id)
        {
            throw new NotImplementedException();
        }

        public int UpdateTruong(TruongHoc truong)
        {
            throw new NotImplementedException();
        }
    }
}
