﻿using AmazingTech.InternSystem.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace swp391_be.API.Models.Request.Authenticate
{
    public class RegisterUserRequestDTO
    {
        [Required(ErrorMessage = "HoVaTen is required"), MinLength(2), MaxLength(30)]
        public string HoVaTen { get; set; }
        
        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please input a right phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "username is required"), MinLength(2), MaxLength(30)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public string Truong { get; set; }

        public string Mssv { get; set; }
    }
}
