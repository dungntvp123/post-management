using System;
using System.Collections.Generic;

namespace PRN_Project.Models;

public partial class Emojy
{
    public int EmojiId { get; set; }

    public int? UserId { get; set; }

    public int? PostId { get; set; }

    public string? Content { get; set; }

    public DateTime? EmoteDate { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? User { get; set; }
}
