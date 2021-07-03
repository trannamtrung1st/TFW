using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TAuth.ResourceClient.Models.Resource;

namespace TAuth.ResourceClient.Services
{
    public interface IResourceService
    {
        Task<IEnumerable<ResourceListItemModel>> GetAsync();
        Task<ResourceDetailModel> GetAsync(int id);
        Task<int> CreateAsync(CreateResourceModel model);
        Task DeleteAsync(int id);
    }

    public class ResourceService : IResourceService
    {
        private readonly HttpClient _httpClient;

        public ResourceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateAsync(CreateResourceModel model)
        {
            var resp = await _httpClient.PostAsJsonAsync("/api/resources", model);

            if (!resp.IsSuccessStatusCode) throw new Exception("Failed to create resource");

            var createdId = await resp.Content.ReadFromJsonAsync<int>();
            return createdId;
        }

        public async Task DeleteAsync(int id)
        {
            var resp = await _httpClient.DeleteAsync($"/api/resources/{id}");

            if (!resp.IsSuccessStatusCode) throw new Exception("Failed to delete resource");
        }

        public async Task<IEnumerable<ResourceListItemModel>> GetAsync()
        {
            var list = await _httpClient.GetFromJsonAsync<IEnumerable<ResourceListItemModel>>("/api/resources");
            return list;
        }

        public async Task<ResourceDetailModel> GetAsync(int id)
        {
            var item = await _httpClient.GetFromJsonAsync<ResourceDetailModel>($"/api/resources/{id}");
            return item;
        }
    }
}
