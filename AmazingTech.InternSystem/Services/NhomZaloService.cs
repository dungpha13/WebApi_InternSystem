﻿using AmazingTech.InternSystem.Data.Entity;
using AmazingTech.InternSystem.Models.DTO;
using AmazingTech.InternSystem.Repositories.NhomZaloManagement;
using AutoMapper;

namespace AmazingTech.InternSystem.Services
{
    public class NhomZaloService : INhomZaloService
    {
        private readonly INhomZaloRepo _nhomZaloRepo;
        private readonly IMapper _mapper;

        public NhomZaloService(INhomZaloRepo nhomZaloRepo, IMapper mapper)
        {
            _nhomZaloRepo = nhomZaloRepo;
            _mapper = mapper;
        }

        public async Task<List<NhomZalo>> GetAllZaloAsync()
        {
            return await _nhomZaloRepo.GetAllZaloAsync();
        }

        public async Task<NhomZalo?> GetGroupByIdAsync(string id)
        {
            return await _nhomZaloRepo.GetGroupByIdAsync(id);
        }

        public async Task<int> AddNewZaloAsync(string user, NhomZaloDTO zaloDTO)
        {
            NhomZalo nhomZalo = _mapper.Map<NhomZalo>(zaloDTO);
            return await _nhomZaloRepo.AddNewZaloAsync(user, nhomZalo);
        }

        public async Task<int> UpdateZaloAsync(string id, string user, NhomZaloDTO zaloDTO)
        {
            NhomZalo nhomZalo = _mapper.Map<NhomZalo>(zaloDTO);
            return await _nhomZaloRepo.UpdateZaloAsync(id, user, nhomZalo);
        }

        public async Task<int> DeleteZaloAsync(string id, string user)
        {
            return await _nhomZaloRepo.DeleteZaloAsync(id, user);
        }

        public async Task<List<UserNhomZalo>> GetUsersInGroupAsync(string nhomZaloId)
        {
            return await _nhomZaloRepo.GetUsersInGroupAsync(nhomZaloId);
        }

        public async Task<UserNhomZalo?> GetUserInGroupAsync(string nhomZaloId, string userId)
        {
            return await _nhomZaloRepo.GetUserInGroupAsync(nhomZaloId, userId);
        }

        public async Task<int> AddUserToGroupAsync(string nhomZaloId, string user, UserNhomZaloDTO userDTO)
        {
            UserNhomZalo userNhomZalo = _mapper.Map<UserNhomZalo>(userDTO);
            return await _nhomZaloRepo.AddUserToGroupAsync(nhomZaloId, user, userNhomZalo);
        }

        public async Task<int> UpdateUserInGroupAsync(string nhomZaloId, string user, UserNhomZaloDTO updatedUserDTO)
        {
            UserNhomZalo nhomZalo = _mapper.Map<UserNhomZalo>(updatedUserDTO);
            return await _nhomZaloRepo.UpdateUserInGroupAsync(nhomZaloId, user, nhomZalo);
        }

        public async Task<int> RemoveUserFromGroupAsync(string nhomZaloId, string user, string userId)
        {
            return await _nhomZaloRepo.RemoveUserFromGroupAsync(nhomZaloId, user, userId);
        }
    }
}
