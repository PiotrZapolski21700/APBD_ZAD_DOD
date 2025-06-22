using EventManagementApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApi.Data;

public class EventDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<EventSpeaker> EventSpeakers { get; set; }
    public DbSet<EventParticipant> EventParticipants { get; set; }
    
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure composite keys
        modelBuilder.Entity<EventSpeaker>()
            .HasKey(es => new { es.EventId, es.SpeakerId });
            
        modelBuilder.Entity<EventParticipant>()
            .HasKey(ep => new { ep.EventId, ep.ParticipantId });
        
        // Configure relationships
        modelBuilder.Entity<EventSpeaker>()
            .HasOne(es => es.Event)
            .WithMany(e => e.EventSpeakers)
            .HasForeignKey(es => es.EventId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<EventSpeaker>()
            .HasOne(es => es.Speaker)
            .WithMany(s => s.EventSpeakers)
            .HasForeignKey(es => es.SpeakerId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Event)
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<EventParticipant>()
            .HasOne(ep => ep.Participant)
            .WithMany(p => p.EventParticipants)
            .HasForeignKey(ep => ep.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Add unique constraint on Speaker email
        modelBuilder.Entity<Speaker>()
            .HasIndex(s => s.Email)
            .IsUnique();
            
        // Add unique constraint on Participant email
        modelBuilder.Entity<Participant>()
            .HasIndex(p => p.Email)
            .IsUnique();
        
        // Seed data
        var seedDate = new DateTime(2024, 6, 22, 12, 0, 0, DateTimeKind.Utc);
        
        modelBuilder.Entity<Speaker>().HasData(
            new Speaker { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan.kowalski@example.com", Bio = "Expert in .NET", CreatedAt = seedDate },
            new Speaker { Id = 2, FirstName = "Anna", LastName = "Nowak", Email = "anna.nowak@example.com", Bio = "Cloud architect", CreatedAt = seedDate },
            new Speaker { Id = 3, FirstName = "Piotr", LastName = "Wiśniewski", Email = "piotr.wisniewski@example.com", Bio = "DevOps specialist", CreatedAt = seedDate }
        );
        
        modelBuilder.Entity<Event>().HasData(
            new Event 
            { 
                Id = 1, 
                Title = "Konferencja .NET 2025", 
                Description = "Największa konferencja .NET w Polsce", 
                EventDate = new DateTime(2025, 9, 15, 9, 0, 0), 
                MaxParticipants = 200,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Event 
            { 
                Id = 2, 
                Title = "Warsztaty Azure", 
                Description = "Praktyczne warsztaty z chmury Azure", 
                EventDate = new DateTime(2025, 10, 20, 10, 0, 0), 
                MaxParticipants = 50,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );
        
        modelBuilder.Entity<EventSpeaker>().HasData(
            new EventSpeaker { EventId = 1, SpeakerId = 1, AssignedAt = seedDate },
            new EventSpeaker { EventId = 1, SpeakerId = 2, AssignedAt = seedDate },
            new EventSpeaker { EventId = 2, SpeakerId = 2, AssignedAt = seedDate }
        );
    }
} 