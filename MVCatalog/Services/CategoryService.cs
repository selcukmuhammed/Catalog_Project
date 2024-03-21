using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCatalog.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCatalog.Services
{
	public class CategoryService
	{
		private readonly IConfiguration _config;
		private readonly string _url;
		private readonly HttpClient _client;

		public CategoryService(IConfiguration config)
		{
			_config = config;
			_client = new HttpClient();
			_url = _config.GetValue<string>("URL:Api");
			_client.BaseAddress = new Uri(_url);
		}
		public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
		{
			using var request = new HttpRequestMessage(new HttpMethod("GET"), _url + "/category/GetAllCategory/");
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
			List<CategoryViewModel> categoryList = JsonConvert.DeserializeObject<List<CategoryViewModel>>(responseJson);

			return categoryList;

		}

		public async Task<CategoryViewModel> GetCategoryByIdAsync(long id)
		{
			using var request = new HttpRequestMessage(new HttpMethod("GET"), _url + "/category/GetCategoryById/" + id);
			//request.Content = new StringContent(JsonConvert.SerializeObject(product));
			//request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return null;

			if (!response.IsSuccessStatusCode)
				return null;

			if (response.Content == null)
				return null;

			var responJson = response.Content.ReadAsStringAsync().Result;
			var categoryList = JsonConvert.DeserializeObject<CategoryViewModel>(responJson);

			return categoryList;
		}

		public async Task<bool> AddCategoryAsync(CategoryViewModel category)
		{
			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/category/AddCategory");
			request.Content = new StringContent(JsonConvert.SerializeObject(category));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return false;

			if (!response.IsSuccessStatusCode)
				return false;

			if (response.Content == null)
				return false;

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool categoryList = JsonConvert.DeserializeObject<bool>(responJson);

			return categoryList;
		}

		public async Task<bool> UpdateCategoryAsync(CategoryViewModel category)
		{
			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/category/UpdateCategory");
			request.Content = new StringContent(JsonConvert.SerializeObject(category));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return false;

			if (!response.IsSuccessStatusCode)
				return false;

			if (response.Content == null)
				return false;

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool categoryList = JsonConvert.DeserializeObject<bool>(responJson);

			return categoryList;
		}

		public async Task<bool> DeleteCategoryAsync(long id)
		{
			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/category/DeleteCategory/" + id);
			request.Content = new StringContent(JsonConvert.SerializeObject(id));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return false;

			if (!response.IsSuccessStatusCode)
				return false;

			if (response.Content == null)
				return false;

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool categoryList = JsonConvert.DeserializeObject<bool>(responJson);

			return categoryList;
		}

	}
}
