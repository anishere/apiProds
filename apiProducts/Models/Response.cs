﻿namespace apiProducts.Models
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string? StatusMessage { get; set; }

        public List<ProductsPcLaptop>? listproducts { get; set; }

        public List<Order>? listorders { get; set; }

        public List<Account> listaccounts { get; set; }

        public List<Blog> ListBlogs { get; set; }

        public List<ProductsCPU>? listcpu { get; set; }

        public List<ProductsKeyboard>? listKeyBoard { get; set; }

        public List<ProductsMouse>? listMouse { get; set; }

        public List<ProductsRAM>? listram { get; set; }

        public List<ProductsTaiNghe>? listTaiNghe { get; set; }

        public List<Message>? listMessage { get; set; }

        public List<InfoShop>? InfoShop { get; set; }

        public List<About>? About { get; set; }

        public int TotalCount { get; set; } 

        public List<string> Brands {  get; set; }

        public int BrandProductCount { get; set; }
    }
}
