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
        [Route("GetAllAccounts")]
        public Response GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "SELECT IdTaiKhoan, UserName, Password, Email, PhanQuyen, SDT, Image FROM Accounts ORDER BY IdTaiKhoan";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Account account = new Account();
                        account.IdTaiKhoan = Convert.ToInt32(reader["IdTaiKhoan"]);
                        account.UserName = Convert.ToString(reader["UserName"]);
                        account.Password = Convert.ToString(reader["Password"]);
                        account.Email = Convert.ToString(reader["Email"]);
                        account.PhanQuyen = Convert.ToString(reader["PhanQuyen"]);
                        account.SDT = Convert.ToString(reader["SDT"]);
                        account.Image = Convert.ToString(reader["Image"]);
                        accounts.Add(account);
                    }
                }

                Response response = new Response();
                if (accounts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Accounts found";
                    response.listaccounts = accounts;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No accounts found";
                    response.listaccounts = null;
                }

                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.listaccounts = null;
                return response;
            }
            finally
            {
                connection.Close();
            }
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

        [HttpPost]
        [Route("updateAccount")]
        public Boolean UpdateAccount(Account updatedAccount)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString()))
            {
                string updateQuery = "UPDATE Accounts SET UserName = @UserName, Email = @Email, SDT = @SDT, Image = @Image " +
                                     "WHERE IdTaiKhoan = @IdTaiKhoan";

                SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
                updateCmd.Parameters.AddWithValue("@UserName", updatedAccount.UserName);
                updateCmd.Parameters.AddWithValue("@Email", updatedAccount.Email);
                updateCmd.Parameters.AddWithValue("@SDT", updatedAccount.SDT);
                updateCmd.Parameters.AddWithValue("@Image", updatedAccount.Image);
                updateCmd.Parameters.AddWithValue("@IdTaiKhoan", updatedAccount.IdTaiKhoan);

                connection.Open();
                int rowsAffected = updateCmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        [HttpPut]
        [Route("UpdateRole")]
        public Response UpdateRole(int idTaiKhoan, string newRole)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "UPDATE Accounts SET PhanQuyen = @PhanQuyen WHERE IdTaiKhoan = @IdTaiKhoan";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhanQuyen", newRole);
                    cmd.Parameters.AddWithValue("@IdTaiKhoan", idTaiKhoan);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    Response response = new Response();
                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Role updated successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Account not found";
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                return response;
            }
            finally
            {
                connection.Close();
            }
        }


        [HttpDelete]
        [Route("DeleteAccount/{id}")]
        public Response DeleteAccount(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                string query = "DELETE FROM Accounts WHERE IdTaiKhoan = @IdTaiKhoan";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@IdTaiKhoan", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    Response response = new Response();
                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Account deleted successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Account not found";
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                return response;
            }
            finally
            {
                connection.Close();
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
