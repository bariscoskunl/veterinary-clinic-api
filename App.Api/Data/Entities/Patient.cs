using System.ComponentModel.DataAnnotations;

namespace App.Api.Data.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string AnimalName { get; set; }

        [Required, MaxLength(50)]
        public string Species { get; set; }

        [Required, MaxLength(50)]
        public string Breed { get; set; }

        [Required, MaxLength(100)]
        public string OwnerName { get; set; }

        public string? TreatmentDescription { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        public bool IsVaccinated { get; set; }

    }
}
