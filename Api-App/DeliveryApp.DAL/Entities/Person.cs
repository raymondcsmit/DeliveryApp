using Core;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace DeliveryApp.DAL.Entities
{
    public class User : IUser<int>
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; } // Add phone number property
        public bool PhoneNumberConfirmed { get; set; } // Add phone number confirmed property
        public bool TwoFactorEnabled { get; set; } // Add two-factor authentication enabled property
        [NotMapped]
        public virtual ICollection<UsersRole> UserRoles { get; set; } = new List<UsersRole>();
        // Additional fields as required by your application
        public short UserTypeId { get; set; }
        // Add additional fields as required by your application
    }
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        [NotMapped]
        public ICollection<UsersRole> UserRoles { get; set; }
    }
    public class UsersRole
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public short CityId { get; set; }
        public string Street { get; set; }
        public string Road { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public short? AddressTypeId { get; set; }
    }

}
