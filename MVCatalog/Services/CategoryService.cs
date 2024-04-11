using Microsoft.AspNetCore.Http;
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
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CategoryService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
		{
			_config = config;
			_client = new HttpClient();
			_url = _config.GetValue<string>("URL:Api");
			_client.BaseAddress = new Uri(_url);
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<ResponseModel<List<CategoryViewModel>>> GetAllCategoryAsync()
		{
			ResponseModel<List<CategoryViewModel>> responseModel = new ResponseModel<List<CategoryViewModel>>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("GET"), _url + "/category/GetAllCategory/");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			//request.Content = new StringContent(JsonConvert.SerializeObject(data));
			//request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return responseModel;

			if (response.StatusCode.ToString() == "401")
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}

			if (!response.IsSuccessStatusCode)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}
			if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				responseModel.StatusCode = "403";
				responseModel.Description = "Hata kodu : 403.Bu alana giriş yetkiniz yoktur.";
				return responseModel;
			}

			if (response.Content == null)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
				return responseModel;
			}

			var responseJson = response.Content.ReadAsStringAsync().Result;

			List<CategoryViewModel> categoryList = JsonConvert.DeserializeObject<List<CategoryViewModel>>(responseJson);
			responseModel.Result = new List<CategoryViewModel>();
			responseModel.Result = categoryList;

			return responseModel;

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

		public async Task<ResponseModel<bool>> AddCategoryAsync(CategoryViewModel category)
		{
			ResponseModel<bool> responseModel = new ResponseModel<bool>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/category/AddCategory");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			request.Content = new StringContent(JsonConvert.SerializeObject(category));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return responseModel;

			if (response.StatusCode.ToString() == "401")
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}

			if (!response.IsSuccessStatusCode)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}
			if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				responseModel.StatusCode = "403";
				responseModel.Description = "Hata kodu : 403.Bu alana giriş yetkiniz yoktur.";
				return responseModel;
			}

			if (response.Content == null)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
				return responseModel;
			}

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool categoryList = JsonConvert.DeserializeObject<bool>(responJson);

			responseModel.Result = new bool();
			responseModel.Result = categoryList;

			return responseModel;
		}

		public async Task<ResponseModel<bool>> UpdateCategoryAsync(CategoryViewModel category)
		{
			ResponseModel<bool> responseModel = new ResponseModel<bool>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/category/UpdateCategory");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			request.Content = new StringContent(JsonConvert.SerializeObject(category));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return responseModel;

			if (response.StatusCode.ToString() == "401")
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}

			if (!response.IsSuccessStatusCode)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}
			if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				responseModel.StatusCode = "403";
				responseModel.Description = "Hata kodu : 403.Bu alana giriş yetkiniz yoktur.";
				return responseModel;
			}

			if (response.Content == null)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
				return responseModel;
			}

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool categoryList = JsonConvert.DeserializeObject<bool>(responJson);

			responseModel.Result = new bool();
			responseModel.Result = categoryList;

			return responseModel;
		}

		public async Task<ResponseModel<bool>> DeleteCategoryAsync(long id)
		{
			ResponseModel<bool> responseModel = new ResponseModel<bool>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/category/DeleteCategory/" + id);
			request.Content = new StringContent(JsonConvert.SerializeObject(id));
			request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

			var response = _client.Send(request);

			if (response == null)
				return responseModel;

			if (response.StatusCode.ToString() == "401")
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}

			if (!response.IsSuccessStatusCode)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
			}
			if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
			{
				responseModel.StatusCode = "403";
				responseModel.Description = "Hata kodu : 403.Bu alana giriş yetkiniz yoktur.";
				return responseModel;
			}

			if (response.Content == null)
			{
				responseModel.StatusCode = response.StatusCode.ToString();
				return responseModel;
			}

			var responJson = response.Content.ReadAsStringAsync().Result;
			bool categoryList = JsonConvert.DeserializeObject<bool>(responJson);

			responseModel.Result = new bool();
			responseModel.Result = categoryList;

			return responseModel;
		}

	}
}
