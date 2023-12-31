﻿using Dapper;
using Microsoft.Data.SqlClient;

namespace Vehicle.API.Application.Queries
{
    public class VehicleQueries : IVehicleQueries
    {
        private string _connectionString;

        public VehicleQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehicleLocationAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select v.[Id] as VehicleId, v.CurrentLocation 
                        FROM Vehicle v"
                );

                return MapVehicleList(result);
            }
        }

        public async Task<Vehicle> GetVehicleLocationAsync(Guid vehicleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select v.[Id] as VehicleId, v.CurrentLocation 
                        FROM Vehicle v
                        WHERE v.Id = @vehicleId"
                    , new { vehicleId }
                );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapVehicle(result);
            }
        }

        public async Task<VehicleLocationHistory> GetVehicleLocationHistoryAsync(Guid vehicleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select v.[Id] as VehicleId, v.CurrentLocation, l.Date, l.Coordinates
                        FROM Vehicle v
                        LEFT JOIN Location l
                            ON l.VehicleId = v.Id
                        WHERE v.Id = @vehicleId
                        ORDER BY l.Date DESC"
                    , new { vehicleId }
                );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapVehicleLocationHistory(result);
            }
        }

        public async Task<VehicleOrders> GetVehicleOrdersAsync(Guid vehicleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select v.[Id] as VehicleId, o.OrderId, o.DateAdded 
                        FROM Vehicle v
                        LEFT JOIN [Order] o
                            ON o.VehicleId = v.Id
                            AND o.DateRemoved IS NULL
                        WHERE v.Id = @vehicleId"
                    , new { vehicleId }
                );

                return MapVehicleOrderList(result);
            }
        }

        public async Task<Vehicle> GetVehicleByOrderAsync(Guid orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<dynamic>(
                    @"select v.[Id] as VehicleId, v.CurrentLocation 
                        FROM Vehicle v
                        INNER JOIN [Order] o
                            ON o.VehicleId = v.Id
                            AND o.DateRemoved IS NULL
                        WHERE o.OrderId = @orderId"
                    , new { orderId }
                );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapVehicle(result);
            }
        }

        private IEnumerable<Vehicle> MapVehicleList(dynamic result)
        {
            var vehicleList = new List<Vehicle>();

            foreach (dynamic item in result)
            {
                var vehicleLocation = new Vehicle
                {
                    VehicleId = item.VehicleId,
                    CurrentLocation = item.CurrentLocation
                };

                vehicleList.Add(vehicleLocation);
            }

            return vehicleList;
        }

        private Vehicle MapVehicle(dynamic result)
        {
            var vehicleLocation = new Vehicle
            {
                VehicleId = result[0].VehicleId,
                CurrentLocation = result[0].CurrentLocation
            };

            return vehicleLocation;
        }

        private VehicleLocationHistory MapVehicleLocationHistory(dynamic result)
        {
            var vehicleLocationHistory = new VehicleLocationHistory
            {
                VehicleId = result[0].VehicleId,
                LocationHistory = new List<Location>()
            };

            foreach (dynamic item in result)
            {
                var locationHistory = new Location
                {
                    Date = item.Date,
                    Coordinates = item.Coordinates
                };

                vehicleLocationHistory.LocationHistory.Add(locationHistory);
            }

            return vehicleLocationHistory;
        }

        private VehicleOrders MapVehicleOrderList(dynamic result)
        {
            var vehicleOrders = new VehicleOrders
            {
                VehicleId = result[0].VehicleId,
                Orders = new List<VehicleOrder>()
            };

            foreach (dynamic item in result)
            {
                if (item.OrderId != null)
                {
                    var vehicleOrder = new VehicleOrder
                    {
                        OrderId = item.OrderId,
                        DateAdded = item.DateAdded
                    };

                    vehicleOrders.Orders.Add(vehicleOrder);
                }
            }

            return vehicleOrders;
        }
    }
}
