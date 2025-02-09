namespace FinanceManagerApi.Models.Response
{
    public class BadRequestDto
    {
        public string Message { get; set; } = null!;
        public List<string> Errors { get; set; } = null!;
    }
}
