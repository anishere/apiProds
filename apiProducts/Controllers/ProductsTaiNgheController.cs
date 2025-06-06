﻿using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsTaiNgheController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsTaiNgheController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllTaiNghe")]
        public Response GetAllTaiNghe()
        {
            List<ProductsTaiNghe> lstproducts = new List<ProductsTaiNghe>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsTaiNghe ORDER BY ProductID", connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsTaiNghe products = new ProductsTaiNghe();
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
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.TanSo = Convert.ToString(dt.Rows[i]["TanSo"]);
                    products.KetNoi = Convert.ToString(dt.Rows[i]["KetNoi"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.Microphone = Convert.ToString(dt.Rows[i]["Microphone"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listTaiNghe = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listTaiNghe = null;
            }

            return response;
        }


        [HttpGet]
        [Route("ListTaiNghe")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsTaiNghe> lstproducts = new List<ProductsTaiNghe>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsTaiNghe ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsTaiNghe products = new ProductsTaiNghe();
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
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.TanSo = Convert.ToString(dt.Rows[i]["TanSo"]);
                    products.KetNoi = Convert.ToString(dt.Rows[i]["KetNoi"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.Microphone = Convert.ToString(dt.Rows[i]["Microphone"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listTaiNghe = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listTaiNghe = null;
            }

            return response;
        }

        [HttpGet]
        [Route("SearchTaiNghe")]
        public Response SearchTaiNghe(string keyword, int page = 1, int pageSize = 20)
        {
            List<ProductsTaiNghe> lstproducts = new List<ProductsTaiNghe>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                // Truy vấn đếm tổng số sản phẩm TaiNghe khớp với từ khóa tìm kiếm
                string countQuery = "SELECT COUNT(*) FROM ProductsTaiNghe WHERE ProductName LIKE @Keyword";
                int totalCount = 0;

                using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                {
                    countCmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    totalCount = (int)countCmd.ExecuteScalar();
                }

                // Truy vấn lấy thông tin sản phẩm TaiNghe với phân trang
                string query = @"
            SELECT * FROM ProductsTaiNghe 
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
                        ProductsTaiNghe products = new ProductsTaiNghe();
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
                        products.Type = Convert.ToString(reader["Type"]);
                        products.BaoHanh = Convert.ToString(reader["BaoHanh"]);
                        products.TanSo = Convert.ToString(reader["TanSo"]);
                        products.KetNoi = Convert.ToString(reader["KetNoi"]);
                        products.KieuKetNoi = Convert.ToString(reader["KieuKetNoi"]);
                        products.MauSac = Convert.ToString(reader["MauSac"]);
                        products.DenLed = Convert.ToString(reader["DenLed"]);
                        products.Microphone = Convert.ToString(reader["Microphone"]);
                        products.KhoiLuong = Convert.ToString(reader["KhoiLuong"]);
                        products.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);
                        lstproducts.Add(products);
                    }
                }

                Response response = new Response();
                if (lstproducts.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Products found";
                    response.listTaiNghe = lstproducts;
                    response.TotalCount = totalCount; // Thêm số lượng tổng cộng
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
                    response.listTaiNghe = null;
                    response.TotalCount = 0; // Thêm số lượng tổng cộng
                }

                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.listTaiNghe = null;
                response.TotalCount = 0; // Thêm số lượng tổng cộng
                return response;
            }
            finally
            {
                connection.Close();
            }
        }


        [HttpGet]
        [Route("GetTaiNgheById/{id}")]
        public Response GetTaiNgheById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsTaiNghe WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductsTaiNghe product = new ProductsTaiNghe();
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
                        product.TanSo = Convert.ToString(reader["TanSo"]);
                        product.KetNoi = Convert.ToString(reader["KetNoi"]);
                        product.KieuKetNoi = Convert.ToString(reader["KieuKetNoi"]);
                        product.MauSac = Convert.ToString(reader["MauSac"]);
                        product.DenLed = Convert.ToString(reader["DenLed"]);
                        product.Microphone = Convert.ToString(reader["Microphone"]);
                        product.KhoiLuong = Convert.ToString(reader["KhoiLuong"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listTaiNghe = new List<ProductsTaiNghe> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listTaiNghe = null;
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

                string query = "SELECT COUNT(*) FROM ProductsTaiNghe";

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

                string query = "SELECT COUNT(*) FROM ProductsTaiNghe WHERE Brand = @Brand";

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
        [Route("GetAllTaiNgheBrands")]
        public Response GetAllTaiNgheBrands()
        {
            List<string> lstBrands = new List<string>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Brand FROM ProductsTaiNghe", connection);

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
        [Route("GetTaiNgheByBrand")]
        public Response GetTaiNgheByBrand(string brand, int page = 1, int pageSize = 20)
        {
            List<ProductsTaiNghe> lstproducts = new List<ProductsTaiNghe>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsTaiNghe WHERE Brand = @Brand ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
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
                    ProductsTaiNghe products = new ProductsTaiNghe();
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
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.BaoHanh = Convert.ToString(dt.Rows[i]["BaoHanh"]);
                    products.TanSo = Convert.ToString(dt.Rows[i]["TanSo"]);
                    products.KetNoi = Convert.ToString(dt.Rows[i]["KetNoi"]);
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.MauSac = Convert.ToString(dt.Rows[i]["MauSac"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.Microphone = Convert.ToString(dt.Rows[i]["Microphone"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listTaiNghe = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listTaiNghe = null;
            }

            return response;
        }


        [HttpPost]
        [Route("AddTaiNghe")]
        public Response AddProduct(ProductsTaiNghe obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsTaiNghe (ProductName, Description, Brand, Discount, Price, Image, Image2, Image3, Image4, Type, BaoHanh, TanSo, KetNoi, KieuKetNoi, MauSac, DenLed, Microphone, KhoiLuong, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @Image2, @Image3, @Image4, @Type, @BaoHanh, @TanSo, @KetNoi, @KieuKetNoi, @MauSac, @DenLed, @Microphone, @KhoiLuong, @NgayNhap)";

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
                    cmd.Parameters.AddWithValue("@TanSo", obj.TanSo);
                    cmd.Parameters.AddWithValue("@KetNoi", obj.KetNoi);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", obj.KieuKetNoi);
                    cmd.Parameters.AddWithValue("@MauSac", obj.MauSac);
                    cmd.Parameters.AddWithValue("@DenLed", obj.DenLed);
                    cmd.Parameters.AddWithValue("@Microphone", obj.Microphone);
                    cmd.Parameters.AddWithValue("@KhoiLuong", obj.KhoiLuong);
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
        [Route("UpdateTaiNghe/{id}")]
        public Response UpdateProduct(int id, ProductsTaiNghe updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsTaiNghe " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, Image2 = @Image2, Image3 = @Image3, Image4 = @Image4, Type = @Type, " +
                               "BaoHanh = @BaoHanh, TanSo = @TanSo, KetNoi = @KetNoi, " +
                               "KieuKetNoi = @KieuKetNoi, MauSac = @MauSac, DenLed = @DenLed, " +
                               "Microphone = @Microphone, KhoiLuong = @KhoiLuong, NgayNhap = @NgayNhap " +
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
                    cmd.Parameters.AddWithValue("@TanSo", updatedProduct.TanSo);
                    cmd.Parameters.AddWithValue("@KetNoi", updatedProduct.KetNoi);
                    cmd.Parameters.AddWithValue("@KieuKetNoi", updatedProduct.KieuKetNoi);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@DenLed", updatedProduct.DenLed);
                    cmd.Parameters.AddWithValue("@Microphone", updatedProduct.Microphone);
                    cmd.Parameters.AddWithValue("@KhoiLuong", updatedProduct.KhoiLuong);
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
        [Route("DeleteTaiNghe/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsTaiNghe WHERE ProductID = @ProductID";

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
