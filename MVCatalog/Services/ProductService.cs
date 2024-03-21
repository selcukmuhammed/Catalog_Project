using Microsoft.AspNetCore.Mvc;
using MVCatalog.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCatalog.Services
{
    public class ProductService
    {
        private readonly IConfiguration _config;
        private readonly string _url;
        private readonly HttpClient _client;

        public ProductService(IConfiguration config)
        {
            _config = config;
            _client = new HttpClient();
            _url = _config.GetValue<string>("URL:Api");
			_client.BaseAddress = new Uri(_url);
		}

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            using var request = new HttpRequestMessage(new HttpMethod("GET"), _url + "/product/GetAllProducts/");
            //request.Content = new StringContent(JsonConvert.SerializeObject(data));
            //request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = _client.Send(request);

            if (response == null)
                return null;


            if (!response.IsSuccessStatusCode)
                return null;

            if (response.Content == null)
                return null;

            var responseJson = response.Content.ReadAsStringAsync().Result;

            List<ProductViewModel> productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(responseJson);

            return productList;

        }

		public async Task<ProductViewModel> GetProductByIdAsync(long id)
		{
			using var request = new HttpRequestMessage(new HttpMethod("GET"), _url + "/product/GetProductById/" + id);
			//request.Content = new StringContent(JsonConvert.SerializeObject(id));
			//request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return null;

			if (!response.IsSuccessStatusCode)
				return null;

			if (response.Content == null)
				return null;

			var responseJson = response.Content.ReadAsStringAsync().Result;
			var productList = JsonConvert.DeserializeObject<ProductViewModel>(responseJson);

			return productList;
		}

		public async Task<bool> AddProductAsync(ProductViewModel product)
		{
			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/product/AddProduct");
			request.Content = new StringContent(JsonConvert.SerializeObject(product));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return false;

			if (!response.IsSuccessStatusCode)
				return false;

			if (response.Content == null)
				return false;

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool productList = JsonConvert.DeserializeObject<bool>(responJson);

			return productList;
		}

		public async Task<bool> UpdateProductAsync(ProductViewModel product)
		{
			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/product/UpdateProduct");
			request.Content = new StringContent(JsonConvert.SerializeObject(product));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return false;

			if (!response.IsSuccessStatusCode)
				return false;

			if (response.Content == null)
				return false;

			var responseJson = response.Content.ReadAsStringAsync().Result;
			bool productList = JsonConvert.DeserializeObject<bool>(responseJson);

			return productList;
		}

		public async Task<bool> DeleteProductAsync(long id)
		{
			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/product/DeleteProduct/" + id);
			request.Content = new StringContent(JsonConvert.SerializeObject(id));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return false;

			if (!response.IsSuccessStatusCode)	
				return false;

			if (response.Content == null)
				return false;

			var responseJson = response.Content.ReadAsStringAsync().Result;
			bool productList = JsonConvert.DeserializeObject<bool>(responseJson);

			return productList;
		}
	}
}
