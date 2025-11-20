namespace mkBoutiqueCaftan.Models;

public class ReservationArticle
{
    public int IdReservation { get; set; }
    public int IdArticle { get; set; }
    public int Quantite { get; set; } = 1;
    
    // Navigation properties
    public Reservation? Reservation { get; set; }
    public Article? Article { get; set; }
}

