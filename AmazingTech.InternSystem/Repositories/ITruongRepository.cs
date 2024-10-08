﻿using AmazingTech.InternSystem.Data.Entity;

namespace AmazingTech.InternSystem.Repositories
{
    public interface ITruongRepository
    {

        TruongHoc? GetTruong(string id);
        List<TruongHoc> GetAllTruongs();
        int AddTruong(TruongHoc truong);
        int UpdateTruong(TruongHoc truong);
        Task<int> DeleteTruong(TruongHoc truong, string uId);
    }
}
