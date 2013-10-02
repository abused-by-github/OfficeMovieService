namespace Svitla.MovieService.WebUI.Models
{
    public class ResponseObject
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }

        public ResponseObject(bool status, string errorMessage, object data)
        {
            Status = status;
            ErrorMessage = errorMessage;
            Data = data;
        }
    }
}