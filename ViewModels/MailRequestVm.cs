using System;
using System.ComponentModel.DataAnnotations;

namespace Laboris.ViewModels
{
    public class MailRequestVm
    {
        public string? ToEmail { get; set; }

        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        public string Subject { get; set; }

        public string Comment { get; set; }

        public List<IFormFile>? Attachments { get; set; }
    }
}

