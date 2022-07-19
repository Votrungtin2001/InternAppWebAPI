using System;
using System.Collections.Generic;

namespace InternAppWebAPI.Models
{
    public partial class RosenUser
    {
        public int UserId { get; set; }
        public string? UserFirstName { get; set; }
        public string? UserLastName { get; set; }
        public DateTime? UserDob { get; set; }
        public string? UserGender { get; set; }
        public string? UserCompany { get; set; }
        public int? UserTitleId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserImage { get; set; }
        public long? UserCreatedDate { get; set; }

        public virtual Title? UserTitle { get; set; }
    }

    public sealed class DataModelSearchUser
    {
        public int titleId { get; set; }

        public string searchText { get; set; }

    }
}
