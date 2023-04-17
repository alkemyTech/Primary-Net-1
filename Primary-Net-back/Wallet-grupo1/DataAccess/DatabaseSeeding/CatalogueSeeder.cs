using Microsoft.EntityFrameworkCore;
using Wallet_grupo1.Entities;

namespace Wallet_grupo1.DataAccess.DatabaseSeeding;

public class CatalogueSeeder : IEntitySeeder
{
    public void SeedTheDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Catalogue>().HasData(
            new Catalogue{
                Id = 1, Name = "Pelota de Fútbol" ,
                Image = "https://d28hi93gr697ol.cloudfront.net/586d6ca1-ace0-eb27-9e0b-2d0d3c3c6062/img/Producto/e200c95c-2e6a-7dcf-052d-2f80a2668c75/parg1-632112923f995.png" ,
                Points = 1555
            },
            new Catalogue{
                Id = 2, Name = "Pava eléctrica" , 
                Image = "https://images.fravega.com/f500/89e9482e3c0cb5a987245974b808e49a.jpg" , 
                Points = 2651
            },
            new Catalogue{ 
                Id = 3, Name = "Paragüas Ugo Renzo" , 
                Image = "https://http2.mlstatic.com/D_NQ_NP_680104-MLA45995632766_052021-V.jpg" , 
                Points = 1251},
            new Catalogue{
                Id = 4, Name = "Hollow Knight Edición Coleccionista" , 
                Image = "https://external-preview.redd.it/lH6EbyzRNwHFxriEyUpV6vUXdDO7FH3HJwzA5uncxgo.jpg?auto=webp&s=aacfcb757b1e7f8e168337de9fcc07fe9d3b017b" ,
                Points = 10000
            },
            new Catalogue{
                Id = 5, Name = "Fernet Branca" ,
                Image = "https://cdn.shopify.com/s/files/1/0588/1380/8845/products/FERNET_BRANCA_750_79e0adc2-315d-4cbd-aece-15097ac92e66_1024x1024.jpg?v=1656944450" , 
                Points = 400
            },
            new Catalogue{
                Id = 6, Name = "Fender Stratocaster Deluxe" ,
                Image = "https://d2r9epyceweg5n.cloudfront.net/stores/001/087/816/products/fender-deluxe-roadhouse-stratocaster-11-d657fc15c97f1e0a0115762512032032-640-0.jpg" ,
                Points = 800000
            }
        );
    }
}