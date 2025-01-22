namespace FinanceManagerApi.Models.ProfileImage
{
    public class ProfileImageDto
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string? Caption { get; set; }
    }
}
