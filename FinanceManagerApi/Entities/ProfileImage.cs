namespace FinanceManagerApi.Entities
{
    public class ProfileImage
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;
        public string? Caption { get; set; } 
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
