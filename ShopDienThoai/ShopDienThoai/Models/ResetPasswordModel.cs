﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopDienThoai.Models
{
  public class ResetPasswordModel
  {
    public string? Email { get; set; }

    [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string ResetCode { get; set; }
  }
}