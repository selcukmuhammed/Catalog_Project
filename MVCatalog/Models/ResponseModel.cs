namespace MVCatalog.Models
{
	public class ResponseModel<T>
	{
		public string StatusCode { get; set; }
		public string Description { get; set; }
		public T Result { get; set; }
	}
}
