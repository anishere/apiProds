using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsRAMController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsRAMController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllRAM")]
        public Response GetAllRAM()
        {
            List<ProductsRAM> lstproducts = new List<ProductsRAM>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsRAM ORDER BY ProductID", connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsRAM products = new ProductsRAM();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Image2 = Convert.ToString(dt.Rows[i]["Image2"]);
                    products.Image3 = Convert.ToString(dt.Rows[i]["Image3"]);
                    products.Image4 = Convert.ToString(dt.Rows[i]["Image4"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.TheHe = Convert.ToString(dt.Rows[i]["TheHe"]);
                    products.Bus = Convert.ToString(dt.Rows[i]["Bus"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.LoaiHang = Convert.ToString(dt.Rows[i]["LoaiHang"]);
                    products.PartNumber = Convert.ToString(dt.Rows[i]["PartNumber"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.DungLuong = Convert.ToString(dt.Rows[i]["DungLuong"]);
                    products.Vol = Convert.ToString(dt.Rows[i]["Vol"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listram = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listram = null;
            }

            return response;
        }


        [HttpGet]
        [Route("ListRAM")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsRAM> lstproducts = new List<ProductsRAM>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsRAM ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsRAM products = new ProductsRAM();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Image2 = Convert.ToString(dt.Rows[i]["Image2"]);
                    products.Image3 = Convert.ToString(dt.Rows[i]["Image3"]);
                    products.Image4 = Convert.ToString(dt.Rows[i]["Image4"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.TheHe = Convert.ToString(dt.Rows[i]["TheHe"]);
                    products.Bus = Convert.ToString(dt.Rows[i]["Bus"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.LoaiHang = Convert.ToString(dt.Rows[i]["LoaiHang"]);
                    products.PartNumber = Convert.ToString(dt.Rows[i]["PartNumber"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.DungLuong = Convert.ToString(dt.Rows[i]["DungLuong"]);
                    products.Vol = Convert.ToString(dt.Rows[i]["Vol"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listram = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listram = null;
            }

            return response;
        }

        [HttpGet]
        [Route("SearchRAM")]
        public Response SearchRAM(string keyword, int page = 1, int pageSize = 20)
        {
            List<ProductsRAM> lstproducts = new List<ProductsRAM>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                // Truy vấn đếm tổng số sản phẩm RAM khớp với từ khóa tìm kiếm
                string countQuery = "SELECT COUNT(*) FROM ProductsRAM WHERE ProductName LIKE @Keyword";
                int totalCount = 0;

                using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                {
                    countCmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    totalCount = (int)countCmd.ExecuteScalar();
                }

                // Truy vấn lấy thông tin sản phẩm RAM với phân trang
                string query = @"
            SELECT * FROM ProductsRAM 
            WHERE ProductName LIKE @Keyword 
            ORDER BY ProductID 
            OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    cmd.Parameters.AddWithValue("@StartIndex", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductsRAM products = new ProductsRAM();
                        products.ProductID = Convert.ToInt32(reader["ProductID"]);
                        products.ProductName = Convert.ToString(reader["ProductName"]);
                        products.Description = Convert.ToString(reader["Description"]);
                        products.Brand = Convert.ToString(reader["Brand"]);
                        products.Discount = Convert.ToDecimal(reader["Discount"]);
                        products.Price = Convert.ToDecimal(reader["Price"]);
                        products.Image = Convert.ToString(reader["Image"]);
                        products.Image2 = Convert.ToString(reader["Image2"]);
                        products.Image3 = Convert.ToString(reader["Image3"]);
                        products.Image4 = Convert.ToString(reader["Image4"]);
                        products.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        products.MauSac = Convert.ToString(reader["MauSac"]);
                        products.TheHe = Convert.ToString(reader["TheHe"]);
                        products.Bus = Convert.ToString(reader["Bus"]);
                        products.DenLed = Convert.ToString(reader["DenLed"]);
                        products.LoaiHang = Convert.ToString(reader["LoaiHang"]);
                        products.PartNumber = Convert.ToString(reader["PartNumber"]);
                        products.NhuCau = Convert.ToString(reader["NhuCau"]);
                        products.DungLuong = Convert.ToString(reader["DungLuong"]);
                        products.Vol = Convert.ToString(reader["Vol"]);
                        products.Type = Convert.ToString(reader["Type"]);
                        products.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        lstproducts.Add(products);
                    }
                }

                Response response = new Response();
                if (lstproducts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listram = lstproducts;
                    response.TotalCount = totalCount; // Thêm số lượng tổng cộng
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
                    response.listram = null;
                    response.TotalCount = 0; // Thêm số lượng tổng cộng
                }

                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.listram = null;
                response.TotalCount = 0; // Thêm số lượng tổng cộng
                return response;
            }
            finally
            {
                connection.Close();
            }
        }


        [HttpGet]
        [Route("GetRAMById/{id}")]
        public Response GetRAMById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsRAM WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductsRAM product = new ProductsRAM();
                        product.ProductID = Convert.ToInt32(reader["ProductID"]);
                        product.ProductName = Convert.ToString(reader["ProductName"]);
                        product.Description = Convert.ToString(reader["Description"]);
                        product.Brand = Convert.ToString(reader["Brand"]);
                        product.Discount = Convert.ToDecimal(reader["Discount"]);
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        product.Image = Convert.ToString(reader["Image"]);
                        product.Image2 = Convert.ToString(reader["Image2"]);
                        product.Image3 = Convert.ToString(reader["Image3"]);
                        product.Image4 = Convert.ToString(reader["Image4"]);
                        product.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        product.MauSac = Convert.ToString(reader["MauSac"]);
                        product.TheHe = Convert.ToString(reader["TheHe"]);
                        product.Bus = Convert.ToString(reader["Bus"]);
                        product.DenLed = Convert.ToString(reader["DenLed"]);
                        product.LoaiHang = Convert.ToString(reader["LoaiHang"]);
                        product.PartNumber = Convert.ToString(reader["PartNumber"]);
                        product.NhuCau = Convert.ToString(reader["NhuCau"]);
                        product.DungLuong = Convert.ToString(reader["DungLuong"]);
                        product.Vol = Convert.ToString(reader["Vol"]);
                        product.Type = Convert.ToString(reader["Type"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listram = new List<ProductsRAM> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listram = null;
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

                string query = "SELECT COUNT(*) FROM ProductsRAM";

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

                string query = "SELECT COUNT(*) FROM ProductsRAM WHERE Brand = @Brand";

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
        [Route("GetAllRAMBrands")]
        public Response GetAllRAMBrands()
        {
            List<string> lstBrands = new List<string>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Brand FROM ProductsRAM", connection);

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

        [HttpGet]
        [Route("GetRAMByBrand")]
        public Response GetRAMByBrand(string brand, int page = 1, int pageSize = 20)
        {
            List<ProductsRAM> lstproducts = new List<ProductsRAM>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsRAM WHERE Brand = @Brand ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
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
                    ProductsRAM products = new ProductsRAM();
                    products.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                    products.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    products.Description = Convert.ToString(dt.Rows[i]["Description"]);
                    products.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    products.Discount = Convert.ToDecimal(dt.Rows[i]["Discount"]);
                    products.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    products.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    products.Image2 = Convert.ToString(dt.Rows[i]["Image2"]);
                    products.Image3 = Convert.ToString(dt.Rows[i]["Image3"]);
                    products.Image4 = Convert.ToString(dt.Rows[i]["Image4"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.TheHe = Convert.ToString(dt.Rows[i]["TheHe"]);
                    products.Bus = Convert.ToString(dt.Rows[i]["Bus"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.LoaiHang = Convert.ToString(dt.Rows[i]["LoaiHang"]);
                    products.PartNumber = Convert.ToString(dt.Rows[i]["PartNumber"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.DungLuong = Convert.ToString(dt.Rows[i]["DungLuong"]);
                    products.Vol = Convert.ToString(dt.Rows[i]["Vol"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listram = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listram = null;
            }

            return response;
        }


        [HttpPost]
        [Route("AddRAM")]
        public Response AddProduct(ProductsRAM obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsRAM (ProductName, Description, Brand, Discount, Price, Image, Image2, Image3, Image4, BaoHanh, MauSac, TheHe, Bus, DenLed, LoaiHang, PartNumber, NhuCau, DungLuong, Vol, Type, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @Image2, @Image3, @Image4, @BaoHanh, @MauSac, @TheHe, @Bus, @DenLed, @LoaiHang, @PartNumber, @NhuCau, @DungLuong, @Vol, @Type, @NgayNhap)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductName", obj.ProductName);
                    cmd.Parameters.AddWithValue("@Description", obj.Description);
                    cmd.Parameters.AddWithValue("@Brand", obj.Brand);
                    cmd.Parameters.AddWithValue("@Discount", obj.Discount);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Image", obj.Image);
                    cmd.Parameters.AddWithValue("@Image2", obj.Image2);
                    cmd.Parameters.AddWithValue("@Image3", obj.Image3);
                    cmd.Parameters.AddWithValue("@Image4", obj.Image4);
                    cmd.Parameters.AddWithValue("@BaoHanh", obj.BaoHanh);
                    cmd.Parameters.AddWithValue("@MauSac", obj.MauSac);
                    cmd.Parameters.AddWithValue("@TheHe", obj.TheHe);
                    cmd.Parameters.AddWithValue("@Bus", obj.Bus);
                    cmd.Parameters.AddWithValue("@DenLed", obj.DenLed);
                    cmd.Parameters.AddWithValue("@LoaiHang", obj.LoaiHang);
                    cmd.Parameters.AddWithValue("@PartNumber", obj.PartNumber);
                    cmd.Parameters.AddWithValue("@NhuCau", obj.NhuCau);
                    cmd.Parameters.AddWithValue("@DungLuong", obj.DungLuong);
                    cmd.Parameters.AddWithValue("@Vol", obj.Vol);
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
        [Route("UpdateRAM/{id}")]
        public Response UpdateProduct(int id, ProductsRAM updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsRAM " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, Image2 = @Image2, Image3 = @Image3, Image4 = @Image4, BaoHanh = @BaoHanh, " +
                               "MauSac = @MauSac, TheHe = @TheHe, Bus = @Bus, " +
                               "DenLed = @DenLed, LoaiHang = @LoaiHang, PartNumber = @PartNumber, " +
                               "NhuCau = @NhuCau, DungLuong = @DungLuong, Vol = @Vol, " +
                               "Type = @Type, NgayNhap = @NgayNhap " +
                               "WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cmd.Parameters.AddWithValue("@ProductName", updatedProduct.ProductName);
                    cmd.Parameters.AddWithValue("@Description", updatedProduct.Description);
                    cmd.Parameters.AddWithValue("@Brand", updatedProduct.Brand);
                    cmd.Parameters.AddWithValue("@Discount", updatedProduct.Discount);
                    cmd.Parameters.AddWithValue("@Price", updatedProduct.Price);
                    cmd.Parameters.AddWithValue("@Image", updatedProduct.Image);
                    cmd.Parameters.AddWithValue("@Image2", updatedProduct.Image2);
                    cmd.Parameters.AddWithValue("@Image3", updatedProduct.Image3);
                    cmd.Parameters.AddWithValue("@Image4", updatedProduct.Image4);
                    cmd.Parameters.AddWithValue("@BaoHanh", updatedProduct.BaoHanh);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@TheHe", updatedProduct.TheHe);
                    cmd.Parameters.AddWithValue("@Bus", updatedProduct.Bus);
                    cmd.Parameters.AddWithValue("@DenLed", updatedProduct.DenLed);
                    cmd.Parameters.AddWithValue("@LoaiHang", updatedProduct.LoaiHang);
                    cmd.Parameters.AddWithValue("@PartNumber", updatedProduct.PartNumber);
                    cmd.Parameters.AddWithValue("@NhuCau", updatedProduct.NhuCau);
                    cmd.Parameters.AddWithValue("@DungLuong", updatedProduct.DungLuong);
                    cmd.Parameters.AddWithValue("@Vol", updatedProduct.Vol);
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
        [Route("DeleteRAM/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsRAM WHERE ProductID = @ProductID";

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
