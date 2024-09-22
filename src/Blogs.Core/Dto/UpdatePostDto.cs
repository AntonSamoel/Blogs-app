using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Dto
{
    public class UpdatePostDto
    {
        public int PostId { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public int? CategoryId { get; set; }
    }
}
