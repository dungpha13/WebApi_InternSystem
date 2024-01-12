using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IT.groupzalo
{
  
    public interface IAddNhomZalo
    {

        List<NhomZalo>? GetAllGroup();

        int AddNewGroup(NhomZalo entity);


    }
    public class AddNhomZalo
    {
        private readonly AppDbContext _dbContext;
        public AddNhomZalo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddGroup(NhomZalo nhomZalo)
        {
            _dbContext.NhomZalos.Add(nhomZalo);
            _dbContext.SaveChanges();
        }
        public List<NhomZalo> GetZaloGroupList()
        {
            return _dbContext.NhomZalos.ToList();
        }
    }
    class Program
    {
        static void Main()
        {
            using (var dbContext = new AppDbContext())
            {
                var repository = new AddNhomZalo(dbContext);

                var newZaloGroup = new NhomZalo
                {
                    TenNhom = "Tên Nhóm Mới",
                    LinkNhom = "Link Nhóm",
                    MentorId = "ID's Mentor mới  "

                };
                repository.AddGroup(newZaloGroup);


                var GroupZaloList = repository.GetZaloGroupList();

                foreach (var item in GroupZaloList)
                {
                    Console.WriteLine($"Tên Nhóm : {item.TenNhom}, Link Nhóm : {item.LinkNhom},ID Mentor:{item.Mentor},Tên Mentor :{item.Mentor}");
                }
            }

        }
    }
}

