using Blogs.Core.Models.AuthModels;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Blogs.Core.Dto
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int PostID { get; set; }

    }
}
