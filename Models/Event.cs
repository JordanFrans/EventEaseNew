using System;
using System.ComponentModel.DataAnnotations;

public class Event
{
    [Key]
    public int EventId { get; set; }

    [Required]
    public string Name { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string ImageUrl { get; set; }
}