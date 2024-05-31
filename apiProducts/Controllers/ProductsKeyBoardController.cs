using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsKeyBoardController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsKeyBoardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllKeyboards")]
        public Response GetAllKeyboards()
        {
            List<ProductsKeyboard> lstproducts = new List<ProductsKeyboard>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsKeyboard ORDER BY ProductID", connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsKeyboard products = new ProductsKeyboard();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Image2 = Convert.ToString(dt.Rows[i]["Image2"]);
                    products.Image3 = Convert.ToString(dt.Rows[i]["Image3"]);
                    products.Image4 = Convert.ToString(dt.Rows[i]["Image4"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.Switch = Convert.ToString(dt.Rows[i]["Switch"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.KeTay = Convert.ToString(dt.Rows[i]["KeTay"]);
                    products.KichThuoc = Convert.ToString(dt.Rows[i]["KichThuoc"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listKeyBoard = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listKeyBoard = null;
            }

            return response;
        }


        [HttpGet]
        [Route("ListKeyBoard")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsKeyboard> lstproducts = new List<ProductsKeyboard>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsKeyboard ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsKeyboard products = new ProductsKeyboard();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Image2 = Convert.ToString(dt.Rows[i]["Image2"]);
                    products.Image3 = Convert.ToString(dt.Rows[i]["Image3"]);
                    products.Image4 = Convert.ToString(dt.Rows[i]["Image4"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.Switch = Convert.ToString(dt.Rows[i]["Switch"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.KeTay = Convert.ToString(dt.Rows[i]["KeTay"]);
                    products.KichThuoc = Convert.ToString(dt.Rows[i]["KichThuoc"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listKeyBoard = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listKeyBoard = null;
            }

            return response;
        }

        [HttpGet]
        [Route("GetKeyBoardById/{id}")]
        public Response GetKeyBoardById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsKeyboard WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductsKeyboard product = new ProductsKeyboard();
                        product.ProductID = Convert.ToInt32(reader["ProductID"]);
                        product.ProductName = Convert.ToString(reader["ProductName"]);
                        product.Description = Convert.ToString(reader["Description"]);
                        product.Brand = Convert.ToString(reader["Brand"]);
                        product.Discount = Convert.ToDecimal(reader["Discount"]);
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        product.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        product.Image = Convert.ToString(reader["Image"]);
                        product.Image2 = Convert.ToString(reader["Image2"]);
                        product.Image3 = Convert.ToString(reader["Image3"]);
                        product.Image4 = Convert.ToString(reader["Image4"]);
                        product.Switch = Convert.ToString(reader["Switch"]);
                        product.MauSac = Convert.ToString(reader["MauSac"]);
                        product.KieuKetNoi = Convert.ToString(reader["KieuKetNoi"]);
                        product.DenLed = Convert.ToString(reader["DenLed"]);
                        product.KeTay = Convert.ToString(reader["KeTay"]);
                        product.KichThuoc = Convert.ToString(reader["KichThuoc"]);
                        product.Type = Convert.ToString(reader["Type"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listKeyBoard = new List<ProductsKeyboard> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listKeyBoard = null;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        [HttpGet]
        [Route("TotalCount")]
        public Response GetTotalProductCount()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ProductsKeyboard";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    int totalCount = Convert.ToInt32(cmd.ExecuteScalar());

                    response.StatusCode = 200;
                    response.StatusMessage = "Total product count found";
                    response.TotalCount = totalCount;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        [HttpGet]
        [Route("TotalProdsByBrand")]
        public Response GetTotalProductsByBrand(string brand)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ProductsKeyboard WHERE Brand = @Brand";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Brand", brand);

                    int totalCount = Convert.ToInt32(cmd.ExecuteScalar());

                    response.StatusCode = 200;
                    response.StatusMessage = "Total product count by brand found";
                    response.TotalCount = totalCount;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        [HttpGet]
        [Route("GetKeyboardsByBrand")]
        public Response GetKeyboardsByBrand(string brand, int page = 1, int pageSize = 20)
        {
            List<ProductsKeyboard> lstproducts = new List<ProductsKeyboard>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsKeyboard WHERE Brand = @Brand ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@Brand", brand);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsKeyboard products = new ProductsKeyboard();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Image2 = Convert.ToString(dt.Rows[i]["Image2"]);
                    products.Image3 = Convert.ToString(dt.Rows[i]["Image3"]);
                    products.Image4 = Convert.ToString(dt.Rows[i]["Image4"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.Switch = Convert.ToString(dt.Rows[i]["Switch"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.KeTay = Convert.ToString(dt.Rows[i]["KeTay"]);
                    products.KichThuoc = Convert.ToString(dt.Rows[i]["KichThuoc"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listKeyBoard = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listKeyBoard = null;
            }

            return response;
        }

        [HttpGet]
        [Route("GetAllKeyboardBrands")]
        public Response GetAllKeyboardBrands()
        {
            List<string> lstBrands = new List<string>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Brand FROM ProductsKeyboard", connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    lstBrands.Add(brand);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.Brands = lstBrands;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.Brands = null;
            }

            return response;
        }


        [HttpPost]
        [Route("AddKeyBoard")]
        public Response AddProduct(ProductsKeyboard obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsKeyboard (ProductName, Description, Brand, Discount, Price, BaoHanh, Image, Image2, Image3, Image4, Switch, MauSac, KieuKetNoi, DenLed, KeTay, KichThuoc, Type, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @BaoHanh, @Image, @Image2, @Image3, @Image4, @Switch, @MauSac, @KieuKetNoi, @DenLed, @KeTay, @KichThuoc, @Type, @NgayNhap)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Description", obj.Description);
                    cmd.Parameters.AddWithValue("@Brand", obj.Brand);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@BaoHanh", obj.BaoHanh);
                    cmd.Parameters.AddWithValue("@Image", obj.Image);
                    cmd.Parameters.AddWithValue("@Image2", obj.Image2);
                    cmd.Parameters.AddWithValue("@Image3", obj.Image3);
                    cmd.Parameters.AddWithValue("@Image4", obj.Image4);
                    cmd.Parameters.AddWithValue("@Switch", obj.Switch);
                    cmd.Parameters.AddWithValue("@MauSac", obj.MauSac);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", obj.KieuKetNoi);
                    cmd.Parameters.AddWithValue("@DenLed", obj.DenLed);
                    cmd.Parameters.AddWithValue("@KeTay", obj.KeTay);
                    cmd.Parameters.AddWithValue("@KichThuoc", obj.KichThuoc);
                    cmd.Parameters.AddWithValue("@Type", obj.Type);
                    cmd.Parameters.AddWithValue("@NgayNhap", obj.NgayNhap);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product added successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Failed to add product";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        [HttpPut]
        [Route("UpdateKeyBoard/{id}")]
        public Response UpdateProduct(int id, ProductsKeyboard updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsKeyboard " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, BaoHanh = @BaoHanh ,Image = @Image,Image2 = @Image2,Image3 = @Image3,Image4 = @Image4, Switch = @Switch, " +
                               "MauSac = @MauSac, KieuKetNoi = @KieuKetNoi, DenLed = @DenLed, " +
                               "KeTay = @KeTay, KichThuoc = @KichThuoc, Type = @Type, NgayNhap = @NgayNhap " +
                               "WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cmd.Parameters.AddWithValue("@ProductName", updatedProduct.ProductName);
                    cmd.Parameters.AddWithValue("@Description", updatedProduct.Description);
                    cmd.Parameters.AddWithValue("@Brand", updatedProduct.Brand);
                    cmd.Parameters.AddWithValue("@Discount", updatedProduct.Discount);
                    cmd.Parameters.AddWithValue("@Price", updatedProduct.Price);
                    cmd.Parameters.AddWithValue("@BaoHanh", updatedProduct.BaoHanh);
                    cmd.Parameters.AddWithValue("@Image", updatedProduct.Image);
                    cmd.Parameters.AddWithValue("@Image2", updatedProduct.Image2);
                    cmd.Parameters.AddWithValue("@Image3", updatedProduct.Image3);
                    cmd.Parameters.AddWithValue("@Image4", updatedProduct.Image4);
                    cmd.Parameters.AddWithValue("@Switch", updatedProduct.Switch);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", updatedProduct.KieuKetNoi);
                    cmd.Parameters.AddWithValue("@DenLed", updatedProduct.DenLed);
                    cmd.Parameters.AddWithValue("@KeTay", updatedProduct.KeTay);
                    cmd.Parameters.AddWithValue("@KichThuoc", updatedProduct.KichThuoc);
                    cmd.Parameters.AddWithValue("@Type", updatedProduct.Type);
                    cmd.Parameters.AddWithValue("@NgayNhap", updatedProduct.NgayNhap);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product updated successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found or failed to update";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        [HttpDelete]
        [Route("DeleteKeyBoard/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsKeyboard WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product deleted successfully";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found or failed to delete";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

    }
}
