using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using apiProducts.Models;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using BCrypt.Net;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("account/{id}")]
        public Account GetAccountById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
            {
                string query = "SELECT IdTaiKhoan, UserName, Password, Email, PhanQuyen, SDT, Image " +
                               "FROM Accounts WHERE IdTaiKhoan = @Id";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return new Account
                    {
                        UserName = row["UserName"].ToString(),
                        Password = row["Password"].ToString(),
                        Email = row["Email"].ToString(),
                        PhanQuyen = row["PhanQuyen"].ToString(),
                        SDT = row["SDT"].ToString(),
                        Image = row["Image"].ToString()
                    };
                }
                else
                {
                    return null; // Trả về null nếu không tìm thấy tài khoản với IdTaiKhoan tương ứng
                }
            }
        }


        [HttpPost]
        [Route("login")]
        public LoginResponse Login(Account account)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
            {
                string query = "SELECT IdTaiKhoan, UserName, Password, Email, SDT, PhanQuyen " +
                               "FROM Accounts WHERE UserName = @UserName AND Password = @Password";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", account.UserName);
                cmd.Parameters.AddWithValue("@Password", account.Password);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string phanQuyen = (row["PhanQuyen"].ToString() == "true") ? "admin" : "user";
                    int idTaiKhoan = Convert.ToInt32(row["IdTaiKhoan"]);

                    return new LoginResponse { IdTaiKhoan = idTaiKhoan, PhanQuyen = phanQuyen };
                }
                else
                {
                    return new LoginResponse { PhanQuyen = "flase" };
                }
            }
        }


        [HttpPost]
        [Route("update")]
        public Boolean UpdatePassword(string userName, string currentPassword, string newPassword)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
            {
                // Kiểm tra tài khoản và mật khẩu hiện tại
                string checkQuery = "SELECT COUNT(*) FROM Accounts WHERE UserName = @UserName AND Password = @CurrentPassword";
                SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
                checkCmd.Parameters.AddWithValue("@UserName", userName);
                checkCmd.Parameters.AddWithValue("@CurrentPassword", currentPassword);

                connection.Open();
                int userCount = (int)checkCmd.ExecuteScalar();

                if (userCount > 0)
                {
                    // Nếu tài khoản tồn tại và mật khẩu đúng, cập nhật mật khẩu mới
                    string updateQuery = "UPDATE Accounts SET Password = @NewPassword WHERE UserName = @UserName";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@UserName", userName);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
                else
                {
                    return false; // Thông tin đăng nhập không chính xác
                }
            }
        }

        [HttpPost]
        [Route("register")]
        public Boolean Register(Account newAccount)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
            {
                // Kiểm tra xem UserName đã tồn tại hay chưa
                string checkQuery = "SELECT COUNT(*) FROM Accounts WHERE UserName = @UserName";
                SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
                checkCmd.Parameters.AddWithValue("@UserName", newAccount.UserName);

                connection.Open();
                int userCount = (int)checkCmd.ExecuteScalar();

                if (userCount > 0)
                {
                    return false; // Tên người dùng đã tồn tại
                }
                else
                {
                    // Thêm tài khoản mới vào cơ sở dữ liệu với PhanQuyen mặc định là false
                    string insertQuery = "INSERT INTO Accounts (UserName, Password, Email, PhanQuyen, SDT, Image) " +
                                         "VALUES (@UserName, @Password, @Email, 'false', @SDT, @Image)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@UserName", newAccount.UserName);
                    insertCmd.Parameters.AddWithValue("@Password", newAccount.Password);
                    insertCmd.Parameters.AddWithValue("@Email", newAccount.Email);
                    insertCmd.Parameters.AddWithValue("@SDT", newAccount.SDT);
                    insertCmd.Parameters.AddWithValue("@Image", newAccount.Image); // Chèn giá trị cho cột Image

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }




        /*[HttpPost]
        [Route("login")]

        public Boolean login (Account account)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Accounts WHERE UserName = '"+account.UserName+"' AND Password = '"+account.Password+"'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

        /*[HttpPost]
        [Route("update")]
        
        public Boolean update (string newPassword) 
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            SqlDataAdapter da = new SqlDataAdapter("UPDATE Account SET Password = @NewPassword WHERE UserName = @UserName", connection);
            da.SelectCommand.Parameters.AddWithValue("@NewPassword", newPassword);
            da.SelectCommand.Parameters.AddWithValue("@UserName", "admin");
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    }
}
