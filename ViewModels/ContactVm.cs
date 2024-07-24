using System;
namespace Laboris.ViewModels;

public class ContactVm
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    public string Message { get; set; } = null!;
}

