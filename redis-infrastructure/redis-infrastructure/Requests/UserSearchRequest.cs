namespace redis_infrastructure.Requests
{
    public record UserSearchRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
    }
}
