using Blogs.Core.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blogs.Core.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(Post))]
        public int PostID { get; set; }
        [JsonIgnore]
        public Post Post { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
