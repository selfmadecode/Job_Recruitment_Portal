using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigJobbs.Interfaces
{
    public interface IFile
    {
        bool IsPdf(string file);

        bool IsJpegOrPng(string file);
    }
}
