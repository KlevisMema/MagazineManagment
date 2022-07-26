﻿using System.ComponentModel.DataAnnotations;

namespace MagazineManagment.DTO.ViewModels
{
    public class RoleCreateViewModel
    {
        [Required(ErrorMessage = "Role name field is required")]
        [Display(Name = "Role name")]
        [StringLength(maximumLength:10,MinimumLength =4)]
        public string RoleName { get; set; } = string.Empty;
    }
}