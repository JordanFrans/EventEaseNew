using System.ComponentModel.DataAnnotations;

public class Venue
{
    [Key] // Primary Key
    public int VenueId { get; set; }

    [Required]
    public string Name { get; set; }

    public string Location { get; set; }

    public int Capacity { get; set; }

    public string ImageUrl { get; set; }
}
