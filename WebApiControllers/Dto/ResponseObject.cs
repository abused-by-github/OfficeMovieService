namespace Svitla.MovieService.WebApi.Dto
{
    public class ResponseObject<TData>
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public TData Data { get; set; }

        public ResponseObject(bool status, string errorMessage, TData data)
        {
            Status = status;
            ErrorMessage = errorMessage;
            Data = data;
        }
    }
}