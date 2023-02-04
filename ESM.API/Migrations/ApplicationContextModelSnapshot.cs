﻿// <auto-generated />
using System;
using ESM.API.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ESM.API.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ESM.Data.Models.Candidate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Candidate");
                });

            modelBuilder.Entity("ESM.Data.Models.CandidateExaminationModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("CandidateId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ExaminationId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ModuleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("ExaminationId");

                    b.HasIndex("ModuleId");

                    b.ToTable("CandidateExaminationModule");
                });

            modelBuilder.Entity("ESM.Data.Models.CandidateShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<Guid>("CandidateId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ExaminationShiftId")
                        .HasColumnType("int");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("ExaminationShiftId");

                    b.ToTable("CandidateShift");
                });

            modelBuilder.Entity("ESM.Data.Models.Department", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("FacultyId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("SchoolId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ESM.Data.Models.Examination", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Examination");
                });

            modelBuilder.Entity("ESM.Data.Models.ExaminationShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("int");

                    b.Property<Guid>("ExaminationId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Method")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModuleId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ExaminationId");

                    b.HasIndex("ModuleId");

                    b.HasIndex("RoomId");

                    b.ToTable("ExaminationShift");
                });

            modelBuilder.Entity("ESM.Data.Models.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("SchoolId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("ESM.Data.Models.InvigilatorExaminationModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ExaminationId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("InvigilatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ModuleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ExaminationId");

                    b.HasIndex("InvigilatorId");

                    b.HasIndex("ModuleId");

                    b.ToTable("InvigilatorExaminationModule");
                });

            modelBuilder.Entity("ESM.Data.Models.InvigilatorShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ExaminationShiftId")
                        .HasColumnType("int");

                    b.Property<Guid>("InvigilatorId")
                        .HasColumnType("char(36)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ExaminationShiftId");

                    b.HasIndex("InvigilatorId");

                    b.ToTable("InvigilatorShift");
                });

            modelBuilder.Entity("ESM.Data.Models.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("FacultyId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("ESM.Data.Models.Right", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Rights");
                });

            modelBuilder.Entity("ESM.Data.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ESM.Data.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .HasColumnType("longtext");

                    b.Property<int?>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("ESM.Data.Models.School", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("ESM.Data.Models.TemporaryRight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ExpiredAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("GrantedById")
                        .HasColumnType("char(36)");

                    b.Property<int>("RightId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("GrantedById");

                    b.HasIndex("RightId");

                    b.HasIndex("UserId");

                    b.ToTable("TemporaryRights");
                });

            modelBuilder.Entity("ESM.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RightRole", b =>
                {
                    b.Property<int>("RightsId")
                        .HasColumnType("int");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("char(36)");

                    b.HasKey("RightsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("RightRole");
                });

            modelBuilder.Entity("ESM.Data.Models.CandidateExaminationModule", b =>
                {
                    b.HasOne("ESM.Data.Models.Candidate", "Candidate")
                        .WithMany("ExaminationModules")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Examination", "Examination")
                        .WithMany("CandidatesOfModule")
                        .HasForeignKey("ExaminationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Module", "Module")
                        .WithMany("CandidatesOfExamination")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("Examination");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("ESM.Data.Models.CandidateShift", b =>
                {
                    b.HasOne("ESM.Data.Models.Candidate", "Candidate")
                        .WithMany("CandidateShift")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.ExaminationShift", "ExaminationShift")
                        .WithMany("CandidateShift")
                        .HasForeignKey("ExaminationShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("ExaminationShift");
                });

            modelBuilder.Entity("ESM.Data.Models.Department", b =>
                {
                    b.HasOne("ESM.Data.Models.Faculty", "Faculty")
                        .WithMany("Departments")
                        .HasForeignKey("FacultyId");

                    b.HasOne("ESM.Data.Models.School", "School")
                        .WithMany("Departments")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");

                    b.Navigation("School");
                });

            modelBuilder.Entity("ESM.Data.Models.Examination", b =>
                {
                    b.HasOne("ESM.Data.Models.User", "CreatedBy")
                        .WithMany("Examinations")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("ESM.Data.Models.ExaminationShift", b =>
                {
                    b.HasOne("ESM.Data.Models.Examination", "Examination")
                        .WithMany("ExaminationsShift")
                        .HasForeignKey("ExaminationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId");

                    b.HasOne("ESM.Data.Models.Room", "Room")
                        .WithMany("ExaminationShift")
                        .HasForeignKey("RoomId");

                    b.Navigation("Examination");

                    b.Navigation("Module");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("ESM.Data.Models.Faculty", b =>
                {
                    b.HasOne("ESM.Data.Models.School", "School")
                        .WithMany("Faculties")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("School");
                });

            modelBuilder.Entity("ESM.Data.Models.InvigilatorExaminationModule", b =>
                {
                    b.HasOne("ESM.Data.Models.Examination", "Examination")
                        .WithMany("InvigilatorsOfModule")
                        .HasForeignKey("ExaminationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.User", "Invigilator")
                        .WithMany("ExaminationModules")
                        .HasForeignKey("InvigilatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Module", "Module")
                        .WithMany("InvigilatorsOfExamination")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Examination");

                    b.Navigation("Invigilator");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("ESM.Data.Models.InvigilatorShift", b =>
                {
                    b.HasOne("ESM.Data.Models.User", "CreatedBy")
                        .WithMany("CreatorInvigilatorShift")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.ExaminationShift", "ExaminationShift")
                        .WithMany("InvigilatorShift")
                        .HasForeignKey("ExaminationShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.User", "Invigilator")
                        .WithMany("InvigilatorShift")
                        .HasForeignKey("InvigilatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("ExaminationShift");

                    b.Navigation("Invigilator");
                });

            modelBuilder.Entity("ESM.Data.Models.Module", b =>
                {
                    b.HasOne("ESM.Data.Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("ESM.Data.Models.Role", b =>
                {
                    b.HasOne("ESM.Data.Models.User", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ESM.Data.Models.TemporaryRight", b =>
                {
                    b.HasOne("ESM.Data.Models.User", "GrantedBy")
                        .WithMany("GrantedTemporaryRights")
                        .HasForeignKey("GrantedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Right", "Right")
                        .WithMany("TemporaryRights")
                        .HasForeignKey("RightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.User", "User")
                        .WithMany("TemporaryRights")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrantedBy");

                    b.Navigation("Right");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ESM.Data.Models.User", b =>
                {
                    b.HasOne("ESM.Data.Models.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("ESM.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("ESM.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("ESM.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RightRole", b =>
                {
                    b.HasOne("ESM.Data.Models.Right", null)
                        .WithMany()
                        .HasForeignKey("RightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ESM.Data.Models.Candidate", b =>
                {
                    b.Navigation("CandidateShift");

                    b.Navigation("ExaminationModules");
                });

            modelBuilder.Entity("ESM.Data.Models.Department", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ESM.Data.Models.Examination", b =>
                {
                    b.Navigation("CandidatesOfModule");

                    b.Navigation("ExaminationsShift");

                    b.Navigation("InvigilatorsOfModule");
                });

            modelBuilder.Entity("ESM.Data.Models.ExaminationShift", b =>
                {
                    b.Navigation("CandidateShift");

                    b.Navigation("InvigilatorShift");
                });

            modelBuilder.Entity("ESM.Data.Models.Faculty", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("ESM.Data.Models.Module", b =>
                {
                    b.Navigation("CandidatesOfExamination");

                    b.Navigation("InvigilatorsOfExamination");
                });

            modelBuilder.Entity("ESM.Data.Models.Right", b =>
                {
                    b.Navigation("TemporaryRights");
                });

            modelBuilder.Entity("ESM.Data.Models.Room", b =>
                {
                    b.Navigation("ExaminationShift");
                });

            modelBuilder.Entity("ESM.Data.Models.School", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("Faculties");
                });

            modelBuilder.Entity("ESM.Data.Models.User", b =>
                {
                    b.Navigation("CreatorInvigilatorShift");

                    b.Navigation("ExaminationModules");

                    b.Navigation("Examinations");

                    b.Navigation("GrantedTemporaryRights");

                    b.Navigation("InvigilatorShift");

                    b.Navigation("Roles");

                    b.Navigation("TemporaryRights");
                });
#pragma warning restore 612, 618
        }
    }
}
