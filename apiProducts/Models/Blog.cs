namespace apiProducts.Models
{
    public class Blog
    {
        public int ID { get; set; }
        public string? TenBlog { get; set; }

        public string? Detail {  get; set; }

        public string? Image { get; set;}

        public string? NguoiViet { get; set; }

        public DateTime NgayViet { get; set; }
    }
}
