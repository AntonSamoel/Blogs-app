using Blogs.Core.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blogs.Core.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }


        [ForeignKey(nameof(Category))]
        public int? CategoryID { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }


        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        
        public ICollection<Comment>? Comments { get; set; }

    }
}
