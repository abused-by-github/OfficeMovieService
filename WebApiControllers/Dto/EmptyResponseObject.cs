namespace Svitla.MovieService.WebApi.Dto
{
    public class EmptyResponseObject : ResponseObject<object>
    {
        public EmptyResponseObject(bool status, string errorMessage)
            : base(status, errorMessage, null) { }
    }
}
