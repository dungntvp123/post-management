using System;
using System.Collections.Generic;

namespace PRN_Project.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Credential { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? RegistrationDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
