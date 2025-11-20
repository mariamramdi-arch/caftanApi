using System.Text.Json.Serialization;

namespace mkBoutiqueCaftan.Models;

public class Reservation
{
    public int IdReservation { get; set; }
    public int IdClient { get; set; }
    public DateTime DateReservation { get; set; } = DateTime.Now;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public decimal MontantTotal { get; set; }
    public StatutReservation StatutReservation { get; set; } = StatutReservation.EnAttente;
    public int? IdPaiement { get; set; }
    public int? IdArticle { get; set; }
    public decimal RemiseAppliquee { get; set; } = 0.00m;
    public int IdSociete { get; set; }
    
    // Navigation properties
    public Client? Client { get; set; }
    public Article? Article { get; set; } // Pour compatibilité (ancien champ)
    public ICollection<ReservationArticle> ReservationArticles { get; set; } = new List<ReservationArticle>();
    public Societe? Societe { get; set; }
    
    [JsonIgnore]
    public Paiement? Paiement { get; set; }
}

public class ReservationDto
{
    public int IdReservation { get; set; }
    public int IdClient { get; set; }
    public DateTime DateReservation { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public decimal MontantTotal { get; set; }
    public StatutReservation StatutReservation { get; set; }
    public int? IdPaiement { get; set; }
    public int? IdArticle { get; set; }
    public decimal RemiseAppliquee { get; set; }
    public int IdSociete { get; set; }
    public ClientDto? Client { get; set; }
    public ArticleDto? Article { get; set; } // Pour compatibilité
    public List<ArticleDto> Articles { get; set; } = new List<ArticleDto>();
    public PaiementDto? Paiement { get; set; }
}

public class CreateReservationRequest
{
    public int IdClient { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    public decimal MontantTotal { get; set; }
    public StatutReservation StatutReservation { get; set; } = StatutReservation.EnAttente;
    public int? IdPaiement { get; set; }
    public int? IdArticle { get; set; } // Pour compatibilité (déprécié, utiliser Articles)
    public List<ArticleReservationItem> Articles { get; set; } = new List<ArticleReservationItem>();
    public decimal RemiseAppliquee { get; set; } = 0.00m;
}

public class ArticleReservationItem
{
    public int IdArticle { get; set; }
    public int Quantite { get; set; } = 1;
}

public class UpdateReservationRequest
{
    public int? IdClient { get; set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public decimal? MontantTotal { get; set; }
    public StatutReservation? StatutReservation { get; set; }
    public int? IdPaiement { get; set; }
    public int? IdArticle { get; set; } // Pour compatibilité (déprécié, utiliser Articles)
    public List<ArticleReservationItem>? Articles { get; set; }
    public decimal? RemiseAppliquee { get; set; }
}

