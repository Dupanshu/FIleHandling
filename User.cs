using System.ComponentModel.DataAnnotations;

namespace FIleHandling;

public class User
{
    public int UserId { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? Occupation { get; set; }

    public string? ZipCode { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
