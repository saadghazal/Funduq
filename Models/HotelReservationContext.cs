using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace funduq.Models;

public partial class HotelReservationContext : DbContext
{
    public HotelReservationContext()
    {
    }

    public HotelReservationContext(DbContextOptions<HotelReservationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<CreditCard> CreditCards { get; set; }

    public virtual DbSet<EventReservation> EventReservations { get; set; }

    public virtual DbSet<HomePage> HomePages { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelService> HotelServices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomReservation> RoomReservations { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<SpecialEvent> SpecialEvents { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;database=HotelReservation;uid=root;pwd=Super123@@", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PRIMARY");

            entity.ToTable("City");

            entity.HasIndex(e => e.CityName, "city_name").IsUnique();

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CityName).HasColumnName("city_name");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PRIMARY");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.ContactUsAddress)
                .HasMaxLength(255)
                .HasColumnName("contact_us_address");
            entity.Property(e => e.ContactUsEmail)
                .HasMaxLength(255)
                .HasColumnName("contact_us_email");
            entity.Property(e => e.ContactUsOpenDays)
                .HasMaxLength(255)
                .HasDefaultValueSql("_utf8mb4\\'Sunday - Thursday\\'")
                .HasColumnName("contact_us_open_days");
            entity.Property(e => e.ContactUsOpenHours)
                .HasMaxLength(255)
                .HasDefaultValueSql("_utf8mb4\\'9:00AM - 5:00PM\\'")
                .HasColumnName("contact_us_open_hours");
            entity.Property(e => e.ContactUsPhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("contact_us_phone_number");
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.CardId).HasName("PRIMARY");

            entity.ToTable("CreditCard");

            entity.HasIndex(e => e.UserId, "fk_user_card_id");

            entity.Property(e => e.CardId).HasColumnName("card_id");
            entity.Property(e => e.CardAmount)
                .HasPrecision(10)
                .HasColumnName("card_amount");
            entity.Property(e => e.CardCvv)
                .HasMaxLength(300)
                .HasColumnName("card_cvv");
            entity.Property(e => e.CardExpiryDate).HasColumnName("card_expiry_date");
            entity.Property(e => e.CardNumber)
                .HasMaxLength(300)
                .HasColumnName("card_number");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.CreditCards)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_card_id");
        });

        modelBuilder.Entity<EventReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PRIMARY");

            entity.ToTable("EventReservation");

            entity.HasIndex(e => e.EventId, "fk_event_reserve_id");

            entity.HasIndex(e => e.UserId, "fk_user_event_id");

            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EventDay).HasColumnName("event_day");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10)
                .HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Event).WithMany(p => p.EventReservations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_event_reserve_id");

            entity.HasOne(d => d.User).WithMany(p => p.EventReservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_user_event_id");
        });

        modelBuilder.Entity<HomePage>(entity =>
        {
            entity.HasKey(e => e.HomeId).HasName("PRIMARY");

            entity.ToTable("HomePage");

            entity.HasIndex(e => e.ContactId, "fk_contact_id");

            entity.Property(e => e.HomeId).HasColumnName("home_id");
            entity.Property(e => e.AboutUsParagraph)
                .HasColumnType("text")
                .HasColumnName("about_us_paragraph");
            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.HomeLogoImage)
                .HasMaxLength(300)
                .HasColumnName("home_logo_image");
            entity.Property(e => e.HomeFavicon)
                .HasMaxLength(300)
                .HasColumnName("home_favicon");
            entity.Property(e => e.AboutUsPicture)
                .HasMaxLength(300)
                .HasColumnName("about_us_picture");
            entity.Property(e => e.TestimonialsBackgroundPicture)
                .HasMaxLength(300)
                .HasColumnName("testimonials_background_picture");
            entity.Property(e => e.BackgroundImage)
                .HasMaxLength(300)
                .HasColumnName("background_image");
            entity.Property(e => e.FacebookUrl)
                .HasMaxLength(300)
                .HasColumnName("facebook_url");
            entity.Property(e => e.InstagramUrl)
              .HasMaxLength(300)
              .HasColumnName("instagram_url");
            entity.Property(e => e.XUrl)
             .HasMaxLength(300)
             .HasColumnName("x_url");
            entity.Property(e => e.TopTitle)
            .HasMaxLength(200)
            .HasColumnName("top_title");
            entity.Property(e => e.TopSubtitle)
            .HasMaxLength(200)
            .HasColumnName("top_subtitle");
            entity.Property(e => e.SearchPlacholder)
            .HasMaxLength(200)
            .HasColumnName("search_placeholder");
            entity.Property(e => e.HomeLogoText)
                .HasMaxLength(300)
                .HasColumnName("home_logo_text");
            entity.HasOne(d => d.Contact).WithMany(p => p.HomePages)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_contact_id");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.HotelId).HasName("PRIMARY");

            entity.ToTable("Hotel");

            entity.HasIndex(e => e.HotelCity, "fk_city_id");

            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.HotelCity).HasColumnName("hotel_city");
            entity.Property(e => e.HotelName)
                .HasMaxLength(255)
                .HasColumnName("hotel_name");
            entity.Property(e => e.HotelPicture)
                .HasMaxLength(300)
                .HasColumnName("hotel_picture");
            entity.Property(e => e.IsFeaturedHotel).HasColumnName("is_featured_hotel");
            entity.Property(e => e.NumberOfStars).HasColumnName("number_of_stars");

            entity.HasOne(d => d.HotelCityNavigation).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.HotelCity)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_city_id");
        });

        modelBuilder.Entity<HotelService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("HotelService");

            entity.HasIndex(e => e.HotelId, "fk_service_hotel_id");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.ServiceType)
                .HasMaxLength(255)
                .HasColumnName("service_type");

            entity.HasOne(d => d.Hotel).WithMany(p => p.HotelServices)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_service_hotel_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleType)
                .HasMaxLength(255)
                .HasColumnName("role_type");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PRIMARY");

            entity.ToTable("Room");

            entity.HasIndex(e => e.HotelId, "fk_room_hotel");

            entity.HasIndex(e => e.TypeId, "fk_room_type");

            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.HasOffer).HasColumnName("has_offer");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.IsBreakfastIncluded).HasColumnName("is_breakfast_included");
            entity.Property(e => e.NightPrice)
                .HasPrecision(10)
                .HasColumnName("night_price");
            entity.Property(e => e.NumberOfBeds).HasColumnName("number_of_beds");
            entity.Property(e => e.NumberOfRooms).HasColumnName("number_of_rooms");
            entity.Property(e => e.RoomDescription)
                .HasColumnType("text")
                .HasColumnName("room_description");
            entity.Property(e => e.RoomImage)
                .HasMaxLength(300)
                .HasColumnName("room_image");
            entity.Property(e => e.RoomName)
                .HasMaxLength(250)
                .HasColumnName("room_name");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.HotelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_room_hotel");

            entity.HasOne(d => d.Type).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_room_type");
        });

        modelBuilder.Entity<RoomReservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("PRIMARY");

            entity.ToTable("RoomReservation");

            entity.HasIndex(e => e.RoomId, "fk_room_reserve_id");

            entity.HasIndex(e => e.UserId, "fk_user_reserve_id");

            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date");
            entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10)
                .HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomReservations)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_room_reserve_id");

            entity.HasOne(d => d.User).WithMany(p => p.RoomReservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_user_reserve_id");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PRIMARY");

            entity.ToTable("RoomType");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.RoomType1)
                .HasMaxLength(255)
                .HasColumnName("room_type");
        });

        modelBuilder.Entity<SpecialEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PRIMARY");

            entity.ToTable("SpecialEvent");

            entity.HasIndex(e => e.HotelId, "fk_event_hotel");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.AvailableFrom).HasColumnName("availableFrom");
            entity.Property(e => e.AvailableTo).HasColumnName("availableTo");
            entity.Property(e => e.EventDescription)
                .HasColumnType("text")
                .HasColumnName("event_description");
            entity.Property(e => e.EventPicture)
                .HasMaxLength(300)
                .HasColumnName("event_picture");
            entity.Property(e => e.HotelId).HasColumnName("hotel_id");
            entity.Property(e => e.MaximumTickets).HasColumnName("maximum_tickets");
            entity.Property(e => e.TicketPrice)
                .HasPrecision(10)
                .HasColumnName("ticket_price");

            entity.HasOne(d => d.Hotel).WithMany(p => p.SpecialEvents)
                .HasForeignKey(d => d.HotelId)
                .HasConstraintName("fk_event_hotel");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestimonialId).HasName("PRIMARY");

            entity.ToTable("Testimonial");

            entity.HasIndex(e => e.UserId, "fk_user_test_id");

            entity.Property(e => e.TestimonialId).HasColumnName("testimonial_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserOpinion)
                .HasColumnType("text")
                .HasColumnName("user_opinion");
            
            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_test_id");
            entity.Property(e => e.TestimonialStatus)
                .HasMaxLength(300)
                .HasDefaultValueSql("_utf8mb4\\'Pending\\'")
                .HasColumnName("testimonial_status");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.RoleId, "fk_role_id");

            entity.HasIndex(e => e.UserId, "fk_user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(300)
                .HasColumnName("email");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(300)
                .HasColumnName("user_password");

            entity.HasOne(d => d.Role).WithMany(p => p.UserCredentials)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_role_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserCredentials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("UserProfile");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(300)
                .HasColumnName("phone_number");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(300)
                .HasColumnName("profile_picture");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
