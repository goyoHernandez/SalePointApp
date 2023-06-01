﻿using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalePoint.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        public UserRepository(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StoreUser?> Login(Access access)
        {
            StoreUser? storeUser = new();
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                string? jsonString = JsonSerializer.Serialize(access);
                StringContent? requestContent = new(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage? response = await client.PostAsync("User/Login", requestContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    storeUser = JsonSerializer.Deserialize<StoreUser?>(content, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return storeUser;
        }

        public async Task<List<Rol>?> GetRols()
        {
            List<Rol>? rols = new();
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");
                HttpResponseMessage response = await client.GetAsync("Rol/Get");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                        rols = JsonSerializer.Deserialize<List<Rol>?>(content, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return rols;
        }

        public async Task<int> CreateUser(StoreUser storeUser)
        {
            int userId = 0;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                string? jsonString = JsonSerializer.Serialize(storeUser);
                StringContent? requestContent = new(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage? response = await client.PostAsync("User/Create", requestContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    userId = JsonSerializer.Deserialize<int>(content, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return userId;
        }

        public async Task<IEnumerable<StoreUser>?> GetAllUsers()
        {
            IEnumerable<StoreUser>? storeUsers = null;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");
                HttpResponseMessage response = await client.GetAsync("User/Get");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                        storeUsers = JsonSerializer.Deserialize<IEnumerable<StoreUser>?>(content, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return storeUsers;
        }

        public async Task<StoreUser?> GetUserById(int userId)
        {
            StoreUser? storeUsers = null;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");
                HttpResponseMessage response = await client.GetAsync($"User/Get/ByUserId/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(content))
                        storeUsers = JsonSerializer.Deserialize<StoreUser?>(content, options);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return storeUsers;
        }

        public async Task<int> UpdateUser(StoreUser storeUser)
        {
            int userId = 0;
            try
            {
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                string? jsonString = JsonSerializer.Serialize(storeUser);
                StringContent? requestContent = new(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage? response = await client.PutAsync("User/Update", requestContent);

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    userId = JsonSerializer.Deserialize<int>(content, options);
                }

                return userId;
            }
            catch
            {
                return userId;
            }
        }

        public async Task<int> DeleteUserById(int userId)
        {
            try
            {
                int id = 0;
                HttpClient client = _httpClient.CreateClient("SalePoinApi");

                HttpResponseMessage? response = await client.DeleteAsync($"User/Delete/ByUserId/{userId}");

                if (response != null && response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    id = JsonSerializer.Deserialize<int>(content, options);
                }

                return id;
            }
            catch
            {
                throw;
            }
        }
    }
}