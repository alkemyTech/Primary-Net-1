using System.ComponentModel.DataAnnotations.Schema;

namespace Wallet_grupo1.Entities
{
    /// <summary>
    /// Entidad unidad de Catálogo, compuesta por un campo identificador, un nombre, una imagen descriptiva
    /// del producto expuesto en dicho catálogo y los puntos requeridos para que un usuario este calificado
    /// para obtenerlo en circunstancias promocionales.
    /// </summary>
    public class Catalogue
    {
        [Column("catalogue_id")]
        public int Id { get; set; }
        
        [Column("catalogue_productDesc", TypeName = "VARCHAR(100)")]
        public string? Name { get; set; }   
        
        [Column("catalogue_image", TypeName = "VARCHAR(700)")]
        public string? Image { get; set; }   
        
        [Column("catalogue_points")]
        public int Points { get; set; }   
    }
}