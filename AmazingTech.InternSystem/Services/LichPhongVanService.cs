using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Repositories;

namespace AmazingTech.InternSystem.Services
{
    public class LichPhongVanService
    {
        private readonly LichPhongVanRepository _lichPhongVanRepository;

        public LichPhongVanService(LichPhongVanRepository lichPhongVanRepository)
        {
            _lichPhongVanRepository = lichPhongVanRepository;
        }

        // Thêm các phương thức xử lý logic nghiệp vụ ở đây

        public void AddLichPhongVan(LichPhongVan lichPhongVan)
        {
            var existingLichPhongVan = _lichPhongVanRepository.GetLichPhongVanById(lichPhongVan.Id);


            if (existingLichPhongVan == null)
            {
                _lichPhongVanRepository.AddLichPhongVan(lichPhongVan);
            }
            else
            {
                
                existingLichPhongVan.ThoiGianPhongVan = lichPhongVan.ThoiGianPhongVan;
                existingLichPhongVan.DiaDiemPhongVan = lichPhongVan.DiaDiemPhongVan;
                existingLichPhongVan.DaXacNhanMail = lichPhongVan.DaXacNhanMail;
                existingLichPhongVan.TrangThai = lichPhongVan.TrangThai;
                existingLichPhongVan.KetQua = lichPhongVan.KetQua;
            }

        }

        public void UpdateLichPhongVan(LichPhongVan lichPhongVan)
        {
            // Thực hiện logic kiểm tra nếu cần
            var existingLichPhongVan = _lichPhongVanRepository.GetLichPhongVanById(lichPhongVan.Id);

            if (existingLichPhongVan != null)
            {
                // Xử lý logic nghiệp vụ khi lịch phỏng vấn đã tồn tại
                existingLichPhongVan.ThoiGianPhongVan = lichPhongVan.ThoiGianPhongVan;
                existingLichPhongVan.DiaDiemPhongVan = lichPhongVan.DiaDiemPhongVan;
                existingLichPhongVan.DaXacNhanMail = lichPhongVan.DaXacNhanMail;
                existingLichPhongVan.TrangThai = lichPhongVan.TrangThai;
                existingLichPhongVan.KetQua = lichPhongVan.KetQua;
            }
        }




        
        

    }
}
