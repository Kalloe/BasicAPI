using System;
namespace BasicAPI.Services
{
    public interface IMailService
    {
        void Send(string subject, string message);
    }
}
