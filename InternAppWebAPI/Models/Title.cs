using System;
using System.Collections.Generic;

namespace InternAppWebAPI.Models
{
    public partial class Title
    {
        public Title()
        {
            RosenUsers = new HashSet<RosenUser>();
        }

        public int TitleId { get; set; }
        public string? TitleName { get; set; }

        public virtual ICollection<RosenUser> RosenUsers { get; set; }
    }
}
