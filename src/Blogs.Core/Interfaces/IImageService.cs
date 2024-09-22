using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Interfaces
{
    public interface IImageService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile, string _imagePath);
        void DeleteImage(string imageFileName, string _imagePath);
    }
}
