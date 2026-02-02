namespace Redarbor.Test.Application.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Fax { get; set; }
        public string? Name { get; set; }
        public DateTime? Lastlogin { get; set; }
        public string Password { get; set; } = string.Empty;
        public int PortalId { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }
        public string? Telephone { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
