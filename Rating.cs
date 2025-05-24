using System.ComponentModel.DataAnnotations;

namespace FIleHandling;

public class Rating
{
	public int UserId { get; set; }

    public int MovieId { get; set; }

    public int? Rating1 { get; set; }

    public int? Timestamp { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
