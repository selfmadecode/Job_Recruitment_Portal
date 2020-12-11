using BigJobbs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigJobbs.Services
{
    public class FileServices : IFile
    {
        public bool IsJpegOrPng(string file)
        {
            if (!(file.Contains(".jpg") || file.Contains(".jpeg") || file.Contains(".png")))
                return false;
            else
                return true;
            
        }

        public bool IsPdf(string file)
        {
            if (!file.Contains(".pdf"))
                return false;
            else
                return true;
        }
    }
}