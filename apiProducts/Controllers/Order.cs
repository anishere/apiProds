using apiProducts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace apiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        public ActionResult<Response> GetAllOrders()
        {
            try
            {
                List<Order> orders = new List<Order>();
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product")))
                {
                    connection.Open();
                    string query = "SELECT * FROM [Order] ORDER BY ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order();
                            order.ID = Convert.ToInt32(reader["ID"]);
                            order.PhoneNumber = reader["PhoneNumber"] as string;
                            order.Name = reader["Name"] as string;
                            order.Address = reader["Address"] as string;
                            order.ListCart = reader["ListCart"] as string;
                            order.TotalPrice = reader["TotalPrice"] as decimal?;
                            order.Status = reader["Status"] as string;
                            orders.Add(order);
                        }
                    }
                }

                if (orders.Count > 0)
                    return Ok(new Response { StatusCode = 200, StatusMessage = "Orders found", listorders = orders });
                else
                    return Ok(new Response { StatusCode = 100, StatusMessage = "No orders found", listorders = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { StatusCode = 500, StatusMessage = "An error occurred: " + ex.Message, listorders = null });
            }
        }

        [HttpPost]
        [Route("AddOrder")]
        public ActionResult<Response> AddOrder(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product")))
                {
                    connection.Open();
                    string query = "INSERT INTO [Order] (PhoneNumber, Name, Address, ListCart, TotalPrice, Status) " +
                                   "VALUES (@PhoneNumber, @Name, @Address, @ListCart, @TotalPrice, @Status); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", order.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Name", order.Name);
                        cmd.Parameters.AddWithValue("@Address", order.Address);
                        cmd.Parameters.AddWithValue("@ListCart", order.ListCart);
                        cmd.Parameters.AddWithValue("@TotalPrice", order.TotalPrice ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", order.Status);

                        int newId = Convert.ToInt32(cmd.ExecuteScalar());

                        if (newId > 0)
                            return Ok(new Response { StatusCode = 200, StatusMessage = "Order added successfully" });
                        else
                            return Ok(new Response { StatusCode = 100, StatusMessage = "Failed to add order" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response { StatusCode = 500, StatusMessage = "An error occurred: " + ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteOrder/{id}")]
        public ActionResult<Response> DeleteOrder(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Product")))
                {
                    connection.Open();
                    string query = "DELETE FROM [Order] WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            return Ok(new Response { StatusCode = 200, StatusMessage = "Order deleted successfully" });
                        else
                            return Ok(new Response { StatusCode = 100, StatusMessage = "Order not found or failed to delete" });
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
