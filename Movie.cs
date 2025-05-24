using System.ComponentModel.DataAnnotations;

namespace FIleHandling;

public class Movie
{
	public int MovieId { get; set; }

    public string? Title { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? ImdbLink { get; set; }

    public bool? Action { get; set; }

    public bool? Adventure { get; set; }

    public bool? Comedy { get; set; }

    public bool? Drama { get; set; }

    public bool? Romance { get; set; }

    public bool? Thriller { get; set; }

    public bool? ScienceFiction { get; set; }

    public bool? Animation { get; set; }

    public bool? Fantasy { get; set; }

    public bool? Horror { get; set; }

    public bool? Musical { get; set; }

    public bool? Mystery { get; set; }

    public bool? Documentary { get; set; }

    public bool? War { get; set; }

    public bool? Crime { get; set; }

    public bool? Western { get; set; }

    public bool? FilmNoir { get; set; }

    public bool? Childrens { get; set; }

    public bool? Other { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
