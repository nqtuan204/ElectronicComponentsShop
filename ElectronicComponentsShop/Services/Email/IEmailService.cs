using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicComponentsShop.Services.Email
{
    public interface IEmailService
    {
        void Send(string title, string content, string to);
    }
}
