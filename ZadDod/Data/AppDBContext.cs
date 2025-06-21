using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ZadDod.Data;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<Speaker> Speakers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ZadDod;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Event__3214EC07FC4967C8");

            entity.ToTable("Event");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasMany(d => d.Participants).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventParticipant",
                    r => r.HasOne<Participant>().WithMany()
                        .HasForeignKey("ParticipantId")
                        .HasConstraintName("FK__EventPart__Parti__5DCAEF64"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .HasConstraintName("FK__EventPart__Event__5CD6CB2B"),
                    j =>
                    {
                        j.HasKey("EventId", "ParticipantId").HasName("PK__EventPar__8E66B185ED364B3C");
                        j.ToTable("EventParticipant");
                    });

            entity.HasMany(d => d.Speakers).WithMany(p => p.Events)
                .UsingEntity<Dictionary<string, object>>(
                    "EventSpeaker",
                    r => r.HasOne<Speaker>().WithMany()
                        .HasForeignKey("SpeakerId")
                        .HasConstraintName("FK__EventSpea__Speak__59FA5E80"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .HasConstraintName("FK__EventSpea__Event__59063A47"),
                    j =>
                    {
                        j.HasKey("EventId", "SpeakerId").HasName("PK__EventSpe__FEDABD6568F6CE76");
                        j.ToTable("EventSpeaker");
                    });
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Particip__3214EC076FCEAF86");

            entity.ToTable("Participant");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Speaker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speaker__3214EC0714679BE0");

            entity.ToTable("Speaker");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
