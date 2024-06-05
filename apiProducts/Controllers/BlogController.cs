using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace apiProducts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BlogController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("AddBlog")]
        public ActionResult<Response> AddBlog(Blog blog)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
                {
                    connection.Open();
                    string query = "INSERT INTO Blog (TenBlog, Detail, Image, NguoiViet, NgayViet) VALUES (@TenBlog, @Detail, @Image, @NguoiViet, @NgayViet)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TenBlog", blog.TenBlog ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Detail", blog.Detail ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Image", blog.Image ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NguoiViet", blog.NguoiViet ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NgayViet", blog.NgayViet);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok(new Response { StatusCode = 200, StatusMessage = "Blog added successfully" });
                        }
                        else
                        {
                            return Ok(new Response { StatusCode = 100, StatusMessage = "Failed to add blog" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { StatusCode = 500, StatusMessage = "An error occurred: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllBlogs")]
        public ActionResult<Response> GetAllBlogs()
        {
            List<Blog> blogs = new List<Blog>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT * FROM Blog ORDER BY NgayViet";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Blog blog = new Blog();
                            blog.ID = Convert.ToInt32(reader["ID"]);
                            blog.TenBlog = Convert.ToString(reader["TenBlog"]);
                            blog.Detail = Convert.ToString(reader["Detail"]);
                            blog.Image = Convert.ToString(reader["Image"]);
                            blog.NguoiViet = Convert.ToString(reader["NguoiViet"]);
                            blog.NgayViet = Convert.ToDateTime(reader["NgayViet"]);
                            blogs.Add(blog);
                        }
                    }
                }

                Response response = new Response();
                if (blogs.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Blogs found";
                    response.ListBlogs = blogs;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No blogs found";
                    response.ListBlogs = null;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.ListBlogs = null;
                return StatusCode(500, response);
            }
            finally
            {
                connection.Close();
            }
        }

        [HttpPut]
        [Route("UpdateBlog")]
        public ActionResult<Response> UpdateBlog(Blog blog)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
                {
                    connection.Open();
                    string query = "UPDATE Blog SET TenBlog = @TenBlog, Detail = @Detail, Image = @Image, NguoiViet = @NguoiViet, NgayViet = @NgayViet WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", blog.ID);
                        cmd.Parameters.AddWithValue("@TenBlog", blog.TenBlog ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Detail", blog.Detail ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Image", blog.Image ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NguoiViet", blog.NguoiViet ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NgayViet", blog.NgayViet);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok(new Response { StatusCode = 200, StatusMessage = "Blog updated successfully" });
                        }
                        else
                        {
                            return Ok(new Response { StatusCode = 100, StatusMessage = "Failed to update blog" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { StatusCode = 500, StatusMessage = "An error occurred: " + ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteBlog/{id}")]
        public ActionResult<Response> DeleteBlog(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
                {
                    connection.Open();
                    string query = "DELETE FROM Blog WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok(new Response { StatusCode = 200, StatusMessage = "Blog deleted successfully" });
                        }
                        else
                        {
                            return Ok(new Response { StatusCode = 100, StatusMessage = "Failed to delete blog" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { StatusCode = 500, StatusMessage = "An error occurred: " + ex.Message });
            }
        }


    }
}