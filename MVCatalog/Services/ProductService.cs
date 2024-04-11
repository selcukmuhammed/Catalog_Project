using Microsoft.AspNetCore.Mvc;
using MVCatalog.Models;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVCatalog.Services
{
    public class ProductService
    {
        private readonly IConfiguration _config;
        private readonly string _url;
        private readonly HttpClient _client;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ProductService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
			_httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
            _url = _config.GetValue<string>("URL:Api");
			_client.BaseAddress = new Uri(_url);
		}

        public async Task<ResponseModel<List<ProductViewModel>>> GetAllProductsAsync()
        {
			ResponseModel<List<ProductViewModel>> responseModel = new ResponseModel<List<ProductViewModel>>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("GET"), _url + "/product/GetAllProducts/");
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
			if (response.StatusCode==System.Net.HttpStatusCode.Forbidden)
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

            List<ProductViewModel> productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(responseJson);

			responseModel.Result = new List<ProductViewModel>();
			responseModel.Result = productList;

			return responseModel;

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

		public async Task<ResponseModel<bool>> AddProductAsync(ProductViewModel product)
		{
			ResponseModel<bool> responseModel = new ResponseModel<bool>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/product/AddProduct");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			request.Content = new StringContent(JsonConvert.SerializeObject(product));
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

			var responseJson = response.Content.ReadAsStringAsync().Result;
			bool productList = JsonConvert.DeserializeObject<bool>(responseJson);

			responseModel.Result = new bool();
			responseModel.Result = productList;

			return responseModel;
		}

		public async Task<ResponseModel<bool>> UpdateProductAsync(ProductViewModel product)
		{
			ResponseModel<bool> responseModel = new ResponseModel<bool>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/product/UpdateProduct");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			request.Content = new StringContent(JsonConvert.SerializeObject(product));
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

			var responseJson = response.Content.ReadAsStringAsync().Result;
			bool productList = JsonConvert.DeserializeObject<bool>(responseJson);

			responseModel.Result = new bool();
			responseModel.Result= productList;

			return responseModel;
		}

		public async Task<ResponseModel<bool>> DeleteProductAsync(long id)
		{
			ResponseModel<bool> responseModel = new ResponseModel<bool>();
			var token = _httpContextAccessor.HttpContext.Session.GetString("token");

			if (token == null)
			{
				responseModel.StatusCode = "401";
				return responseModel;
			}

			using var request = new HttpRequestMessage(new HttpMethod("POST"), _url + "/product/DeleteProduct/" + id);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

			var responseJson = response.Content.ReadAsStringAsync().Result;
			bool productList = JsonConvert.DeserializeObject<bool>(responseJson);

			responseModel.Result = new bool();
			responseModel.Result = productList;

			return responseModel;
		}
	}
}
