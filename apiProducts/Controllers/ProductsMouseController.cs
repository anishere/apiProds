﻿using apiProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsMouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductsMouseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllMouse")]
        public Response GetAllMice()
        {
            List<ProductsMouse> lstproducts = new List<ProductsMouse>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsMouse ORDER BY ProductID", connection);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsMouse products = new ProductsMouse();
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
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.KieuCam = Convert.ToString(dt.Rows[i]["KieuCam"]);
                    products.SoNutBam = Convert.ToString(dt.Rows[i]["SoNutBam"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.KichThuoc = Convert.ToString(dt.Rows[i]["KichThuoc"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.DoPhanGiai = Convert.ToString(dt.Rows[i]["DoPhanGiai"]);
                    products.DangCamBien = Convert.ToString(dt.Rows[i]["DangCamBien"]);
                    products.DoNhay = Convert.ToString(dt.Rows[i]["DoNhay"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listMouse = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listMouse = null;
            }

            return response;
        }


        [HttpGet]
        [Route("ListMouse")]
        public Response GetProductsByPage(int page = 1, int pageSize = 20)
        {
            List<ProductsMouse> lstproducts = new List<ProductsMouse>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsMouse ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
            da.SelectCommand.Parameters.AddWithValue("@StartIndex", startIndex);
            da.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);

            DataTable dt = new DataTable();
            da.Fill(dt);

            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductsMouse products = new ProductsMouse();
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
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.KieuCam = Convert.ToString(dt.Rows[i]["KieuCam"]);
                    products.SoNutBam = Convert.ToString(dt.Rows[i]["SoNutBam"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.KichThuoc = Convert.ToString(dt.Rows[i]["KichThuoc"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.DoPhanGiai = Convert.ToString(dt.Rows[i]["DoPhanGiai"]);
                    products.DangCamBien = Convert.ToString(dt.Rows[i]["DangCamBien"]);
                    products.DoNhay = Convert.ToString(dt.Rows[i]["DoNhay"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listMouse = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listMouse = null;
            }

            return response;
        }

        [HttpGet]
        [Route("SearchMouse")]
        public Response SearchMice(string keyword, int page = 1, int pageSize = 20)
        {
            List<ProductsMouse> lstproducts = new List<ProductsMouse>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            try
            {
                connection.Open();

                // Truy vấn đếm tổng số sản phẩm chuột khớp với từ khóa tìm kiếm
                string countQuery = "SELECT COUNT(*) FROM ProductsMouse WHERE ProductName LIKE @Keyword";
                int totalCount = 0;

                using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                {
                    countCmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
                    totalCount = (int)countCmd.ExecuteScalar();
                }

                // Truy vấn lấy thông tin sản phẩm chuột với phân trang
                string query = @"
            SELECT * FROM ProductsMouse 
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
                        ProductsMouse products = new ProductsMouse();
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
                        products.KieuKetNoi = Convert.ToString(reader["KieuKetNoi"]);
                        products.NhuCau = Convert.ToString(reader["NhuCau"]);
                        products.KieuCam = Convert.ToString(reader["KieuCam"]);
                        products.SoNutBam = Convert.ToString(reader["SoNutBam"]);
                        products.DenLed = Convert.ToString(reader["DenLed"]);
                        products.KichThuoc = Convert.ToString(reader["KichThuoc"]);
                        products.KhoiLuong = Convert.ToString(reader["KhoiLuong"]);
                        products.DoPhanGiai = Convert.ToString(reader["DoPhanGiai"]);
                        products.DangCamBien = Convert.ToString(reader["DangCamBien"]);
                        products.DoNhay = Convert.ToString(reader["DoNhay"]);
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
                    response.listMouse = lstproducts;
                    response.TotalCount = totalCount; // Thêm số lượng tổng cộng
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No products found";
                    response.listMouse = null;
                    response.TotalCount = 0; // Thêm số lượng tổng cộng
                }

                return response;
            }
            catch (Exception ex)
            {
                Response response = new Response();
                response.StatusCode = 500;
                response.StatusMessage = "An error occurred: " + ex.Message;
                response.listMouse = null;
                response.TotalCount = 0; // Thêm số lượng tổng cộng
                return response;
            }
            finally
            {
                connection.Close();
            }
        }


        [HttpGet]
        [Route("GetMouseById/{id}")]
        public Response GetMouseById(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "SELECT * FROM ProductsMouse WHERE ProductID = @ProductID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        ProductsMouse product = new ProductsMouse();
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
                        product.KieuKetNoi = Convert.ToString(reader["KieuKetNoi"]);
                        product.NhuCau = Convert.ToString(reader["NhuCau"]);
                        product.KieuCam = Convert.ToString(reader["KieuCam"]);
                        product.SoNutBam = Convert.ToString(reader["SoNutBam"]);
                        product.DenLed = Convert.ToString(reader["DenLed"]);
                        product.KichThuoc = Convert.ToString(reader["KichThuoc"]);
                        product.KhoiLuong = Convert.ToString(reader["KhoiLuong"]);
                        product.DoPhanGiai = Convert.ToString(reader["DoPhanGiai"]);
                        product.DangCamBien = Convert.ToString(reader["DangCamBien"]);
                        product.DoNhay = Convert.ToString(reader["DoNhay"]);
                        product.Type = Convert.ToString(reader["Type"]);
                        product.NgayNhap = Convert.ToDateTime(reader["NgayNhap"]);

                        response.StatusCode = 200;
                        response.StatusMessage = "Product found";
                        response.listMouse = new List<ProductsMouse> { product };
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "Product not found";
                        response.listMouse = null;
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

                string query = "SELECT COUNT(*) FROM ProductsMouse";

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

                string query = "SELECT COUNT(*) FROM ProductsMouse WHERE Brand = @Brand";

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
        [Route("GetAllMouseBrands")]
        public Response GetAllMouseBrands()
        {
            List<string> lstBrands = new List<string>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT Brand FROM ProductsMouse", connection);

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
        [Route("GetMouseByBrand")]
        public Response GetMiceByBrand(string brand, int page = 1, int pageSize = 20)
        {
            List<ProductsMouse> lstproducts = new List<ProductsMouse>();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());

            int startIndex = (page - 1) * pageSize;

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM ProductsMouse WHERE Brand = @Brand ORDER BY ProductID OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY", connection);
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
                    ProductsMouse products = new ProductsMouse();
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
                    products.KieuKetNoi = Convert.ToString(dt.Rows[i]["KieuKetNoi"]);
                    products.NhuCau = Convert.ToString(dt.Rows[i]["NhuCau"]);
                    products.KieuCam = Convert.ToString(dt.Rows[i]["KieuCam"]);
                    products.SoNutBam = Convert.ToString(dt.Rows[i]["SoNutBam"]);
                    products.DenLed = Convert.ToString(dt.Rows[i]["DenLed"]);
                    products.KichThuoc = Convert.ToString(dt.Rows[i]["KichThuoc"]);
                    products.KhoiLuong = Convert.ToString(dt.Rows[i]["KhoiLuong"]);
                    products.DoPhanGiai = Convert.ToString(dt.Rows[i]["DoPhanGiai"]);
                    products.DangCamBien = Convert.ToString(dt.Rows[i]["DangCamBien"]);
                    products.DoNhay = Convert.ToString(dt.Rows[i]["DoNhay"]);
                    products.Type = Convert.ToString(dt.Rows[i]["Type"]);
                    products.NgayNhap = Convert.ToDateTime(dt.Rows[i]["NgayNhap"]);
                    lstproducts.Add(products);
                }

                response.StatusCode = 200;
                response.StatusMessage = "Data found";
                response.listMouse = lstproducts;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "No data found";
                response.listMouse = null;
            }

            return response;
        }


        [HttpPost]
        [Route("AddMouse")]
        public Response AddProduct(ProductsMouse obj)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "INSERT INTO ProductsMouse (ProductName, Description, Brand, Discount, Price, Image, Image2, Image3, Image4, BaoHanh, MauSac, KieuKetNoi, NhuCau, KieuCam, SoNutBam, DenLed, KichThuoc, KhoiLuong, DoPhanGiai, DangCamBien, DoNhay, Type, NgayNhap) " +
                               "VALUES (@ProductName, @Description, @Brand, @Discount, @Price, @Image, @Image2, @Image3, @Image4, @BaoHanh, @MauSac, @KieuKetNoi, @NhuCau, @KieuCam, @SoNutBam, @DenLed, @KichThuoc, @KhoiLuong, @DoPhanGiai, @DangCamBien, @DoNhay, @Type, @NgayNhap)";

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
                    cmd.Parameters.AddWithValue("@KieuKetNoi", obj.KieuKetNoi);
                    cmd.Parameters.AddWithValue("@NhuCau", obj.NhuCau);
                    cmd.Parameters.AddWithValue("@KieuCam", obj.KieuCam);
                    cmd.Parameters.AddWithValue("@SoNutBam", obj.SoNutBam);
                    cmd.Parameters.AddWithValue("@DenLed", obj.DenLed);
                    cmd.Parameters.AddWithValue("@KichThuoc", obj.KichThuoc);
                    cmd.Parameters.AddWithValue("@KhoiLuong", obj.KhoiLuong);
                    cmd.Parameters.AddWithValue("@DoPhanGiai", obj.DoPhanGiai);
                    cmd.Parameters.AddWithValue("@DangCamBien", obj.DangCamBien);
                    cmd.Parameters.AddWithValue("@DoNhay", obj.DoNhay);
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
        [Route("UpdateMouse/{id}")]
        public Response UpdateProduct(int id, ProductsMouse updatedProduct)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "UPDATE ProductsMouse " +
                               "SET ProductName = @ProductName, Description = @Description, " +
                               "Brand = @Brand, Discount = @Discount, " +
                               "Price = @Price, Image = @Image, Image2 = @Image2, Image3 = @Image3, Image4 = @Image4, BaoHanh = @BaoHanh, " +
                               "MauSac = @MauSac, KieuKetNoi = @KieuKetNoi, NhuCau = @NhuCau, KieuCam = @KieuCam, " +
                               "SoNutBam = @SoNutBam, DenLed = @DenLed, KichThuoc = @KichThuoc, " +
                               "KhoiLuong = @KhoiLuong, DoPhanGiai = @DoPhanGiai, DangCamBien = @DangCamBien, " +
                               "DoNhay = @DoNhay, Type = @Type, NgayNhap = @NgayNhap " +
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
                    cmd.Parameters.AddWithValue("@KieuKetNoi", updatedProduct.KieuKetNoi);
                    cmd.Parameters.AddWithValue("@MauSac", updatedProduct.MauSac);
                    cmd.Parameters.AddWithValue("@NhuCau", updatedProduct.NhuCau);
                    cmd.Parameters.AddWithValue("@KieuCam", updatedProduct.KieuCam);
                    cmd.Parameters.AddWithValue("@SoNutBam", updatedProduct.SoNutBam);
                    cmd.Parameters.AddWithValue("@DenLed", updatedProduct.DenLed);
                    cmd.Parameters.AddWithValue("@KichThuoc", updatedProduct.KichThuoc);
                    cmd.Parameters.AddWithValue("@KhoiLuong", updatedProduct.KhoiLuong);
                    cmd.Parameters.AddWithValue("@DoPhanGiai", updatedProduct.DoPhanGiai);
                    cmd.Parameters.AddWithValue("@DangCamBien", updatedProduct.DangCamBien);
                    cmd.Parameters.AddWithValue("@DoNhay", updatedProduct.DoNhay);
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
        [Route("DeleteMouse/{id}")]
        public Response DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product").ToString());
            Response response = new Response();

            try
            {
                connection.Open();

                string query = "DELETE FROM ProductsMouse WHERE ProductID = @ProductID";

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
