using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigJobbs.Interfaces
{
    public interface IMail
    {
        Task SendMail(string email, string subject, string message);
    }
}
