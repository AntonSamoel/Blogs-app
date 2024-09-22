using System.ComponentModel.DataAnnotations;

namespace Blogs.Core.Models.AuthModels
{
    public class AddRoleModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RoleName { get; set; }

    }
}
