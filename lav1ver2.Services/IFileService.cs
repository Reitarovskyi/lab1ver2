﻿using Microsoft.AspNetCore.Http;

namespace lab1ver2.Services
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
