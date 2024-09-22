using AutoMapper;
using Blogs.Core.Dto;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Mapping
{
    public class PostMapping: Profile
    {
        public PostMapping()
        {
            CreateMap<Post,GetPostDto>();
            CreateMap<PaginationDTO<Post>, PaginationDTO<GetPostDto>>();
        }
    }
}
