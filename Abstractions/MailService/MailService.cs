using System;
using Laboris.ViewModels;

namespace Laboris.Abstractions.MailService
{
	public interface IMailService
	{
        Task SendEmailAsync(MailRequestVm mailRequest);
    }
}

