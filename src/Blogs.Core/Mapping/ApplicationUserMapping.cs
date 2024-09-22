using AutoMapper;
using Blogs.Core.Dto;
using Blogs.Core.Models.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Mapping
{
    public class ApplicationUserMapping: Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, GetApplicationUserDto>();
        }
    }
}
