using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BetterAdminDbAPI.Entities
{
    public partial class BetterAdminContext : DbContext
    {
        public BetterAdminContext()
        {
        }

        public BetterAdminContext(DbContextOptions<BetterAdminContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<Calendar> Calendars { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<ClassRoom> ClassRooms { get; set; } = null!;
        public virtual DbSet<Concert> Concerts { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Enrollment> Enrollments { get; set; } = null!;
        public virtual DbSet<Guardian> Guardians { get; set; } = null!;
        public virtual DbSet<Instrument> Instruments { get; set; } = null!;
        public virtual DbSet<InstrumentType> InstrumentTypes { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<Pupil> Pupils { get; set; } = null!;
        public virtual DbSet<Rental> Rentals { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<WaitList> WaitLists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(25);

                entity.Property(e => e.PostalCode).HasColumnType("int(11)");

                entity.Property(e => e.Road).HasMaxLength(30);
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.HasIndex(e => e.PersonId, "FK_Admin_Person(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Admins)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Admin_Person(ID)");
            });

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.ToTable("Calendar");

                entity.HasIndex(e => e.ClassId, "FK_Calendar_Class(ID)");

                entity.HasIndex(e => e.ConcertId, "FK_Calendar_Concert(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.ClassId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClassID");

                entity.Property(e => e.ConcertId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ConcertID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Calendars)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calendar_Class(ID)");

                entity.HasOne(d => d.Concert)
                    .WithMany(p => p.Calendars)
                    .HasForeignKey(d => d.ConcertId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Calendar_Concert(ID)");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.HasIndex(e => e.CourseId, "FK_Class_Course(ID)");

                entity.HasIndex(e => e.PupilId, "FK_Class_Pupil(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.CourseId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CourseID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.PupilId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PupilID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Class_Course(ID)");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.PupilId)
                    .HasConstraintName("FK_Class_Pupil(ID)");
            });

            modelBuilder.Entity<ClassRoom>(entity =>
            {
                entity.ToTable("ClassRoom");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Concert>(entity =>
            {
                entity.ToTable("Concert");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.Location).HasMaxLength(30);

                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.HasIndex(e => e.ClassRoomId, "FK_Course_ClassRoom(ID)");

                entity.HasIndex(e => e.TeacherId, "FK_Course_Teacher(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.ClassRoomId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ClassRoomID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.MaxEnrolled).HasColumnType("int(11)");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Price).HasPrecision(20, 6);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.TeacherId)
                    .HasColumnType("int(11)")
                    .HasColumnName("TeacherID");

                entity.HasOne(d => d.ClassRoom)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.ClassRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_ClassRoom(ID)");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Course_Teacher(ID)");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollment");

                entity.HasIndex(e => e.CourseId, "FK_Enrollment_Course(ID)");

                entity.HasIndex(e => e.PupilId, "FK_Enrollment_Pupil(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.CourseId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CourseID");

                entity.Property(e => e.PupilId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PupilID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_Course(ID)");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.PupilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollment_Pupil(ID)");
            });

            modelBuilder.Entity<Guardian>(entity =>
            {
                entity.ToTable("Guardian");

                entity.HasIndex(e => e.AddressId, "FK_Guardian_Address(ID)");

                entity.HasIndex(e => e.PersonId, "FK_Guardian_Person(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.AddressId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AddressID");

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonID");

                entity.Property(e => e.WorkPhoneNo).HasMaxLength(12);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Guardians)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Guardian_Address(ID)");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Guardians)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Guardian_Person(ID)");
            });

            modelBuilder.Entity<Instrument>(entity =>
            {
                entity.ToTable("Instrument");

                entity.HasIndex(e => e.Type, "FK_Instrument_InstrumentType");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Price).HasPrecision(20, 6);

                entity.Property(e => e.Type).HasMaxLength(30);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Instruments)
                    .HasPrincipalKey(p => p.Type)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Instrument_InstrumentType");
            });

            modelBuilder.Entity<InstrumentType>(entity =>
            {
                entity.ToTable("InstrumentType");

                entity.HasIndex(e => e.Type, "Type")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Type).HasMaxLength(30);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.HasIndex(e => e.SenderId, "FK1_Message_Person(ID)");

                entity.HasIndex(e => e.ReceiverId, "FK2_Message_Person(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Attachments).HasColumnType("varbinary(8000)");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.ReceiverId)
                    .HasColumnType("int(11)")
                    .HasColumnName("ReceiverID");

                entity.Property(e => e.SenderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("SenderID");

                entity.Property(e => e.TimeSent)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_Message_Person(ID)");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_Message_Person(ID)");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.HasIndex(e => e.EnrollmentId, "FK_Payment_Enrollment(ID)");

                entity.HasIndex(e => e.PersonId, "FK_Payment_Person(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.EnrollmentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("EnrollmentID");

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonID");

                entity.HasOne(d => d.Enrollment)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.EnrollmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Enrollment(ID)");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Person(ID)");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(20);

                entity.Property(e => e.LastName).HasMaxLength(20);

                entity.Property(e => e.PhoneNo).HasMaxLength(12);
            });

            modelBuilder.Entity<Pupil>(entity =>
            {
                entity.ToTable("Pupil");

                entity.HasIndex(e => e.AddressId, "FK_Pupil_Address(ID)");

                entity.HasIndex(e => e.GuardianId, "FK_Pupil_Guardian(ID)");

                entity.HasIndex(e => e.PersonId, "FK_Pupil_Person(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.AddressId)
                    .HasColumnType("int(11)")
                    .HasColumnName("AddressID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.EnrollmentDate).HasColumnType("date");

                entity.Property(e => e.Gender).HasMaxLength(12);

                entity.Property(e => e.Grade).HasColumnType("int(11)");

                entity.Property(e => e.GuardianId)
                    .HasColumnType("int(11)")
                    .HasColumnName("GuardianID");

                entity.Property(e => e.MobileNo).HasMaxLength(12);

                entity.Property(e => e.Note).HasMaxLength(200);

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonID");

                entity.Property(e => e.School).HasMaxLength(50);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Pupils)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pupil_Address(ID)");

                entity.HasOne(d => d.Guardian)
                    .WithMany(p => p.Pupils)
                    .HasForeignKey(d => d.GuardianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pupil_Guardian(ID)");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Pupils)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Pupil_Person(ID)");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.ToTable("Rental");

                entity.HasIndex(e => e.InstrumentId, "FK_Rental_Instrument(ID)");

                entity.HasIndex(e => e.Payer, "FK_Rental_Person(ID)");

                entity.HasIndex(e => e.PupilId, "FK_Rental_Pupil(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.InstrumentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("InstrumentID");

                entity.Property(e => e.Payer).HasColumnType("int(11)");

                entity.Property(e => e.PupilId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PupilID");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Instrument)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.InstrumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rental_Instrument(ID)");

                entity.HasOne(d => d.PayerNavigation)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.Payer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rental_Person(ID)");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.Rentals)
                    .HasForeignKey(d => d.PupilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rental_Pupil(ID)");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.HasIndex(e => e.PersonId, "FK_Teacher_Person(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonID");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("FK_Teacher_Person(ID)");
            });

            modelBuilder.Entity<WaitList>(entity =>
            {
                entity.ToTable("WaitList");

                entity.HasIndex(e => e.CourseId, "FK_WaitList_Course(ID)");

                entity.HasIndex(e => e.PupilId, "FK_WaitList_Pupil(ID)");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("ID");

                entity.Property(e => e.CourseId)
                    .HasColumnType("int(11)")
                    .HasColumnName("CourseID");

                entity.Property(e => e.NumberInQueu).HasColumnType("int(11)");

                entity.Property(e => e.PupilId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PupilID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.WaitLists)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaitList_Course(ID)");

                entity.HasOne(d => d.Pupil)
                    .WithMany(p => p.WaitLists)
                    .HasForeignKey(d => d.PupilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WaitList_Pupil(ID)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
