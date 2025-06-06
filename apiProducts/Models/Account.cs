﻿namespace apiProducts.Models
{
    public class Account
    {
        public int IdTaiKhoan { get; set; }
        public string? Email { get; set; }

        public string? Image { get; set; }

        public string? PhanQuyen { get; set; }

        public string? SDT { get; set; }
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string Visible { get; set; } = "00000000000000";
    }
}
