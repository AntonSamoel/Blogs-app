using Blogs.Core.Models.AuthModels;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Dto
{
    public class GetPostDto
    {
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int? CategoryID { get; set; }
        public string UserId { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
