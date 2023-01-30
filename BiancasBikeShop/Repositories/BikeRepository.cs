using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            var bikes = new List<Bike>();
            using (var connection = Connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT b.*, o.Name AS OwnerName FROM Bike b JOIN Owner o ON b.OwnerId = o.Id";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bikes.Add(new Bike
                    {
                        Id = (int)reader["Id"],
                        Brand = reader["Brand"].ToString(),
                        Color = reader["Color"].ToString(),
                        Owner = new Owner
                        {
                            Name = reader["OwnerName"].ToString()
                        }
                    });
                }
            }
            return bikes;
        }

        private List<WorkOrder> GetWorkOrdersByBikeId(int id)
        {
            var workOrders = new List<WorkOrder>();
            using (var connection = Connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM WorkOrder WHERE BikeId = @id";
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    workOrders.Add(new WorkOrder
                    {
                        Id = (int)reader["Id"],
                        Description = reader["Description"].ToString(),
                        DateInitiated = (DateTime)reader["DateInitiated"],
                        DateCompleted = reader["DateCompleted"] as DateTime?
                    });
                }
            }
            return workOrders;
        }

        public Bike GetBikeById(int id)
        {
            Bike bike = null;
            using (var connection = Connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT b.*, o.Name AS OwnerName, o.Address, t.Name AS BikeType " +
                                    "FROM Bike b " +
                                    "JOIN Owner o ON b.OwnerId = o.Id " +
                                    "JOIN BikeType t ON b.BikeTypeId = t.Id " +
                                    "WHERE b.Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bike = new Bike
                    {
                        Id = (int)reader["Id"],
                        Brand = reader["Brand"].ToString(),
                        Color = reader["Color"].ToString(),
                        Owner = new Owner
                        {
                            Name = reader["OwnerName"].ToString(),
                            Address = reader["Address"].ToString()
                        },
                        BikeType = new BikeType
                        {
                            Name = reader["BikeType"].ToString()
                        }
                    };
                }
            }

            bike.WorkOrders = GetWorkOrdersByBikeId(id);

            return bike;
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            using (var connection = Connection)
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT COUNT(DISTINCT w.BikeId) FROM WorkOrder w " +
                                    "WHERE w.DateCompleted IS NULL";
                count = (int)cmd.ExecuteScalar();
            }
            return count;
        }


    }
}
