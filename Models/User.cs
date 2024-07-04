using System;
using System.Collections.Generic;

namespace PRN_Project.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? AccountId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? NickName { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Emojy> Emojies { get; set; } = new List<Emojy>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

	public override string ToString()
	{
		return $"{"{"}UserId: {UserId}, AccountId: {AccountId}, CreateAt: {CreateDate}{"}"}";
	}
}
