using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace funduq.Models;

public partial class HomePage
{
    public int HomeId { get; set; }

    public string? HomeLogoImage { get; set; }

    public string? HomeLogoText { get; set; }

    public string? AboutUsParagraph { get; set; }

    public string? HomeFavicon { get; set; }

    public string? BackgroundImage { get; set; }

    public string? TopTitle { get; set; }

    public string? TopSubtitle { get; set; }

    public string? SearchPlacholder { get; set; }

    public string? AboutUsPicture { get; set; }

    public string? TestimonialsBackgroundPicture { get; set; }

    public string? FacebookUrl { get; set; }

    public string? InstagramUrl { get; set; }
    public string? XUrl { get; set; }

    [NotMapped]
    public virtual IFormFile LogoFile { get; set; }

    [NotMapped]
    public virtual IFormFile FaviconFile { get; set; }

    [NotMapped]
    public virtual IFormFile BackgroundImageFile { get; set; }

    [NotMapped]
    public virtual IFormFile AboutUsImageFile { get; set; }

    [NotMapped]
    public virtual IFormFile TestimonialsBackgroundImage { get; set; }


    public int? ContactId { get; set; }

    public virtual ContactU? Contact { get; set; }
    
}
