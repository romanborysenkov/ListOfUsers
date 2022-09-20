using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Users.Server.Models
{
    public class User
        {
            [Key]
            public int userId{get; set;}

            [Column(TypeName ="nvarchar(50)")]
            public string userName{get; set;} = string.Empty;

            [Column(TypeName ="nvarchar(50)")]
            public string Email{get; set;}= string.Empty;
        
            [Column(TypeName ="nvarchar(50)")]
            public string Profession{get; set;}= string.Empty;

           
            public  string? imageName{get; set;}= string.Empty;

            [NotMapped]
            public IFormFile? imageFile {get; set;}

            [NotMapped]
            public string? imageSrc {get; set;} 
        }

}