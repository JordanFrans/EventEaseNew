using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    [Key]
    public int BookingId { get; set; }

    [ForeignKey("Venue")]
    public int VenueId { get; set; }

    [ForeignKey("Event")]
    public int EventId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Navigation properties
    public Venue Venue { get; set; }
    public Event Event { get; set; }
}