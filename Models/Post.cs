using System;
using System.Collections.Generic;

namespace PRN_Project.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? UserId { get; set; }

    public string? Content { get; set; }

    public DateTime? PostDate { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Emojy> Emojies { get; set; } = new List<Emojy>();

    public virtual User? User { get; set; }
}
