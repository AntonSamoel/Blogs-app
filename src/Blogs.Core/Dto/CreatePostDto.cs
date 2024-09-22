using Blogs.Core.Models.AuthModels;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Blogs.Core.Dto
{
    public class CreatePostDto
    {
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public int? CategoryId { get; set; }
        
    }
}
