namespace Redarbor.Test.Domain.Entities
{
    public class Employee : BaseEntity
    {
        private Employee() { } // EF Constructor

        public Employee(
            int companyId,
            string email,
            string password,
            int portalId,
            int roleId,
            int statusId,
            string username,
            string? fax,
            string? name,
            string? telephone)
        {
            ValidateRequiredFields(companyId, email, password, portalId, roleId, statusId, username);

            CompanyId = companyId;
            Email = email;
            Password = password;
            PortalId = portalId;
            RoleId = roleId;
            StatusId = statusId;
            Username = username;
            Fax = fax;
            Name = name;
            Telephone = telephone;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public int CompanyId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? DeletedOn { get; private set; }
        public string Email { get; private set; }
        public string? Fax { get; private set; }
        public string? Name { get; private set; }
        public DateTime? Lastlogin { get; private set; }
        public string Password { get; private set; }
        public int PortalId { get; private set; }
        public int RoleId { get; private set; }
        public int StatusId { get; private set; }
        public string? Telephone { get; private set; }
        public DateTime UpdatedOn { get; private set; }
        public string Username { get; private set; }

        public void Update(
            int? companyId = null,
            string? email = null,
            string? password = null,
            int? portalId = null,
            int? roleId = null,
            int? statusId = null,
            string? username = null,
            string? fax = null,
            string? name = null,
            string? telephone = null)
        {
            if (companyId.HasValue) CompanyId = companyId.Value;
            if (!string.IsNullOrWhiteSpace(email)) Email = email;
            if (!string.IsNullOrWhiteSpace(password)) Password = password;
            if (portalId.HasValue) PortalId = portalId.Value;
            if (roleId.HasValue) RoleId = roleId.Value;
            if (statusId.HasValue) StatusId = statusId.Value;
            if (!string.IsNullOrWhiteSpace(username)) Username = username;
            if (!string.IsNullOrWhiteSpace(fax)) Fax = fax;
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
            if (!string.IsNullOrWhiteSpace(telephone)) Telephone = telephone;

            UpdatedOn = DateTime.UtcNow;
        }

        public void SetLastLogin()
        {
            Lastlogin = DateTime.UtcNow;
        }

        public void MarkAsDeleted()
        {
            DeletedOn = DateTime.UtcNow;
        }

        private void ValidateRequiredFields(
            int companyId,
            string email,
            string password,
            int portalId,
            int roleId,
            int statusId,
            string username)
        {
            if (companyId <= 0)
                throw new ArgumentException("CompanyId is required", nameof(companyId));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required", nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required", nameof(password));

            if (portalId <= 0)
                throw new ArgumentException("PortalId is required", nameof(portalId));

            if (roleId <= 0)
                throw new ArgumentException("RoleId is required", nameof(roleId));

            if (statusId <= 0)
                throw new ArgumentException("StatusId is required", nameof(statusId));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required", nameof(username));
        }
    }
}
