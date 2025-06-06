﻿using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsCPUController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public ProductsCPUController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public Response GetAllProducts()
        {
            List<ProductsCPU> lstproducts = new List<ProductsCPU>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsCPU ORDER BY ProductID", connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsCPU products = new ProductsCPU();
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
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.SocKet = Convert.ToString(dt.Rows[i]["SocKet"]);
                    products.SoNhan = Convert.ToString(dt.Rows[i]["SoNhan"]);
                    products.SoLuong = Convert.ToString(dt.Rows[i]["SoLuong"]);
                    products.KienTruc = Convert.ToString(dt.Rows[i]["KienTruc"]);
                    products.TocDo = Convert.ToString(dt.Rows[i]["TocDo"]);
                    products.Cache = Convert.ToString(dt.Rows[i]["Cache"]);
                    products.ChipDoHoa = Convert.ToString(dt.Rows[i]["ChipDoHoa"]);
                    products.TDP = Convert.ToString(dt.Rows[i]["TDP"]);
                    products.BoNhoHoTro = Convert.ToString(dt.Rows[i]["BoNhoHoTro"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listcpu = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listcpu = null;
            }

            return response;
        }

        [HttpGet]
        [Route("ListCPU")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsCPU> lstproducts = new List<ProductsCPU>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsCPU ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsCPU products = new ProductsCPU();
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
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.SocKet = Convert.ToString(dt.Rows[i]["SocKet"]);
                    products.SoNhan = Convert.ToString(dt.Rows[i]["SoNhan"]);
                    products.SoLuong = Convert.ToString(dt.Rows[i]["SoLuong"]);
                    products.KienTruc = Convert.ToString(dt.Rows[i]["KienTruc"]);
                    products.TocDo = Convert.ToString(dt.Rows[i]["TocDo"]);
                    products.Cache = Convert.ToString(dt.Rows[i]["Cache"]);
                    products.ChipDoHoa = Convert.ToString(dt.Rows[i]["ChipDoHoa"]);
                    products.TDP = Convert.ToString(dt.Rows[i]["TDP"]);
                    products.BoNhoHoTro = Convert.ToString(dt.Rows[i]["BoNhoHoTro"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listcpu = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listcpu = null;
            }

            return response;
        }

        [HttpGet]
        [Route("SearchCPUs")]
        public Response SearchCPUs(string keyword, int page = 1, int pageSize = 20)
        {
            List<ProductsCPU> lstproducts = new List<ProductsCPU>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                // Truy vấn đếm tổng số sản phẩm CPU khớp với từ khóa tìm kiếm
                string countQuery = "SELECT COUNT(*) FROM ProductsCPU WHERE ProductName LIKE @Keyword";
                int totalCount = 0;

                using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                {
                    countCmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    totalCount = (int)countCmd.ExecuteScalar();
                }

                // Truy vấn lấy thông tin sản phẩm CPU với phân trang
                string query = @"
            SELECT * FROM ProductsCPU 
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
                        ProductsCPU products = new ProductsCPU();
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
                        products.Type = Convert.ToString(reader["Type"]);
                        products.SocKet = Convert.ToString(reader["SocKet"]);
                        products.SoNhan = Convert.ToString(reader["SoNhan"]);
                        products.SoLuong = Convert.ToString(reader["SoLuong"]);
                        products.KienTruc = Convert.ToString(reader["KienTruc"]);
                        products.TocDo = Convert.ToString(reader["TocDo"]);
                        products.Cache = Convert.ToString(reader["Cache"]);
                        products.ChipDoHoa = Convert.ToString(reader["ChipDoHoa"]);
                        products.TDP = Convert.ToString(reader["TDP"]);
                        products.BoNhoHoTro = Convert.ToString(reader["BoNhoHoTro"]);
                        products.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        lstproducts.Add(products);
                    }
                }

                Response response = new Response();
                if (lstproducts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listcpu = lstproducts;
                    response.TotalCount = totalCount; // Thêm số lượng tổng cộng
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
                    response.listcpu = null;
                    response.TotalCount = 0; // Thêm số lượng tổng cộng
                }

                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.listcpu = null;
                response.TotalCount = 0; // Thêm số lượng tổng cộng
                return response;
            }
            finally
            {
                connection.Close();
            }
        }


        [HttpGet]
        [Route("GetCPUById/{id}")]
        public Response GetCPUById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsCPU WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductsCPU product = new ProductsCPU();
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
                        product.Type = Convert.ToString(reader["Type"]);
                        product.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        product.SocKet = Convert.ToString(reader["SocKet"]);
                        product.SoNhan = Convert.ToString(reader["SoNhan"]);
                        product.SoLuong = Convert.ToString(reader["SoLuong"]);
                        product.KienTruc = Convert.ToString(reader["KienTruc"]);
                        product.TocDo = Convert.ToString(reader["TocDo"]);
                        product.Cache = Convert.ToString(reader["Cache"]);
                        product.ChipDoHoa = Convert.ToString(reader["ChipDoHoa"]);
                        product.TDP = Convert.ToString(reader["TDP"]);
                        product.BoNhoHoTro = Convert.ToString(reader["BoNhoHoTro"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listcpu = new List<ProductsCPU> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listcpu = null;
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

                string query = "SELECT COUNT(*) FROM ProductsCPU";

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

                string query = "SELECT COUNT(*) FROM ProductsCPU WHERE Brand = @Brand";

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
        [Route("GetAllBrands")]
        public Response GetAllBrands()
        {
            List<string> lstBrands = new List<string>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Brand FROM ProductsCPU", connection);

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
        [Route("GetProductsByBrand")]
        public Response GetProductsByBrand(string brand, int page = 1, int pageSize = 20)
        {
            List<ProductsCPU> lstproducts = new List<ProductsCPU>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsCPU WHERE Brand = @Brand ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
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
                    ProductsCPU products = new ProductsCPU();
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
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.SocKet = Convert.ToString(dt.Rows[i]["SocKet"]);
                    products.SoNhan = Convert.ToString(dt.Rows[i]["SoNhan"]);
                    products.SoLuong = Convert.ToString(dt.Rows[i]["SoLuong"]);
                    products.KienTruc = Convert.ToString(dt.Rows[i]["KienTruc"]);
                    products.TocDo = Convert.ToString(dt.Rows[i]["TocDo"]);
                    products.Cache = Convert.ToString(dt.Rows[i]["Cache"]);
                    products.ChipDoHoa = Convert.ToString(dt.Rows[i]["ChipDoHoa"]);
                    products.TDP = Convert.ToString(dt.Rows[i]["TDP"]);
                    products.BoNhoHoTro = Convert.ToString(dt.Rows[i]["BoNhoHoTro"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listcpu = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listcpu = null;
            }

            return response;
        }


        [HttpPost]
        [Route("AddCPU")]
        public Response AddProduct(ProductsCPU obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsCPU (ProductName, Description, Brand, Discount, Price, Image, Image2, Image3, Image4, BaoHanh, Type, SocKet, SoNhan, SoLuong, KienTruc, TocDo, Cache, ChipDoHoa, TDP, BoNhoHoTro, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @Image2, @Image3, @Image4, @BaoHanh, @Type, @SocKet, @SoNhan, @SoLuong, @KienTruc, @TocDo, @Cache, @ChipDoHoa, @TDP, @BoNhoHoTro, @NgayNhap)";

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
                    cmd.Parameters.AddWithValue("@Type", obj.Type);
                    cmd.Parameters.AddWithValue("@BaoHanh", obj.BaoHanh);
                    cmd.Parameters.AddWithValue("@SocKet", obj.SocKet);
                    cmd.Parameters.AddWithValue("@SoNhan", obj.SoNhan);
                    cmd.Parameters.AddWithValue("@SoLuong", obj.SoLuong);
                    cmd.Parameters.AddWithValue("@KienTruc", obj.KienTruc);
                    cmd.Parameters.AddWithValue("@TocDo", obj.TocDo);
                    cmd.Parameters.AddWithValue("@Cache", obj.Cache);
                    cmd.Parameters.AddWithValue("@ChipDoHoa", obj.ChipDoHoa);
                    cmd.Parameters.AddWithValue("@TDP", obj.TDP);
                    cmd.Parameters.AddWithValue("@BoNhoHoTro", obj.BoNhoHoTro);
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
        [Route("UpdateCPU/{id}")]
        public Response UpdateProduct(int id, ProductsCPU updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsCPU " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, Image2 = @Image2, Image3 = @Image3, Image4 = @Image4, Type = @Type, BaoHanh = @BaoHanh, " +
                               "SocKet = @SocKet, SoNhan = @SoNhan, SoLuong = @SoLuong, " +
                               "KienTruc = @KienTruc, TocDo = @TocDo, Cache = @Cache, " +
                               "ChipDoHoa = @ChipDoHoa, TDP = @TDP, BoNhoHoTro = @BoNhoHoTro, NgayNhap = @NgayNhap " +
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
                    cmd.Parameters.AddWithValue("@Type", updatedProduct.Type);
                    cmd.Parameters.AddWithValue("@BaoHanh", updatedProduct.BaoHanh);
                    cmd.Parameters.AddWithValue("@SocKet", updatedProduct.SocKet);
                    cmd.Parameters.AddWithValue("@SoNhan", updatedProduct.SoNhan);
                    cmd.Parameters.AddWithValue("@SoLuong", updatedProduct.SoLuong);
                    cmd.Parameters.AddWithValue("@KienTruc", updatedProduct.KienTruc);
                    cmd.Parameters.AddWithValue("@TocDo", updatedProduct.TocDo);
                    cmd.Parameters.AddWithValue("@Cache", updatedProduct.Cache);
                    cmd.Parameters.AddWithValue("@ChipDoHoa", updatedProduct.ChipDoHoa);
                    cmd.Parameters.AddWithValue("@TDP", updatedProduct.TDP);
                    cmd.Parameters.AddWithValue("@BoNhoHoTro", updatedProduct.BoNhoHoTro);
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
        [Route("DeleteCPU/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsCPU WHERE ProductID = @ProductID";

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
