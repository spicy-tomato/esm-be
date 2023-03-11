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
                .HasAnnotation("ProductVersion", "6.0.14")
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

                    b.ToTable("Candidates");
                });

            modelBuilder.Entity("ESM.Data.Models.CandidateExaminationModule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CandidateId")
                        .HasColumnType("char(36)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<Guid>("ShiftId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CandidateId");

                    b.HasIndex("ShiftId");

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

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ESM.Data.Models.DepartmentShiftGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FacultyShiftGroupId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("FacultyShiftGroupId");

                    b.HasIndex("UserId");

                    b.ToTable("DepartmentShiftGroups");
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

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("DisplayId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ExpectEndAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ExpectStartAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Examinations");
                });

            modelBuilder.Entity("ESM.Data.Models.ExaminationData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int?>("CandidatesCount")
                        .HasColumnType("int");

                    b.Property<int?>("Credit")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Department")
                        .HasColumnType("longtext");

                    b.Property<bool?>("DepartmentAssign")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("EndAt")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ExaminationId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Faculty")
                        .HasColumnType("longtext");

                    b.Property<int?>("Method")
                        .HasColumnType("int");

                    b.Property<string>("ModuleClass")
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleId")
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleName")
                        .HasColumnType("longtext");

                    b.Property<string>("Rooms")
                        .HasColumnType("longtext");

                    b.Property<int?>("RoomsCount")
                        .HasColumnType("int");

                    b.Property<int?>("Shift")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ExaminationId");

                    b.ToTable("ExaminationData");
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

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("ESM.Data.Models.FacultyShiftGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("CalculatedInvigilatorsCount")
                        .HasColumnType("int");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("char(36)");

                    b.Property<int>("InvigilatorsCount")
                        .HasColumnType("int");

                    b.Property<Guid>("ShiftGroupId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("ShiftGroupId");

                    b.ToTable("FacultyShiftGroups");
                });

            modelBuilder.Entity("ESM.Data.Models.InvigilatorShift", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("InvigilatorId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<int>("Paid")
                        .HasColumnType("int");

                    b.Property<Guid>("ShiftId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ShiftId");

                    b.ToTable("InvigilatorShift");
                });

            modelBuilder.Entity("ESM.Data.Models.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("DisplayId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("int");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("FacultyId");

                    b.ToTable("Modules");
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

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                            ConcurrencyStamp = "def3c198-02c6-4eb7-80db-0070e85fda6c",
                            Name = "ExaminationDepartmentHead",
                            NormalizedName = "EXAMINATIONDEPARTMENTHEAD"
                        },
                        new
                        {
                            Id = new Guid("08db1e1a-7953-4790-8ebe-272e34a8fe18"),
                            ConcurrencyStamp = "19bebb2f-26d9-4841-a391-f5d304ebef6c",
                            Name = "Teacher",
                            NormalizedName = "TEACHER"
                        });
                });

            modelBuilder.Entity("ESM.Data.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int?>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("DisplayId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("ESM.Data.Models.Shift", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("CandidatesCount")
                        .HasColumnType("int");

                    b.Property<int>("ExamsCount")
                        .HasColumnType("int");

                    b.Property<int>("InvigilatorsCount")
                        .HasColumnType("int");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ShiftGroupId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("ShiftGroupId");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("ESM.Data.Models.ShiftGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("DepartmentAssign")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ExaminationId")
                        .HasColumnType("char(36)");

                    b.Property<int>("InvigilatorsCount")
                        .HasColumnType("int");

                    b.Property<int>("Method")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModuleId")
                        .HasColumnType("char(36)");

                    b.Property<int>("RoomsCount")
                        .HasColumnType("int");

                    b.Property<int?>("Shift")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ExaminationId");

                    b.HasIndex("ModuleId");

                    b.ToTable("ShiftGroups");
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

                    b.Property<Guid?>("DepartmentId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("InvigilatorId")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsMale")
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

                    b.Property<Guid>("RoleId")
                        .HasColumnType("char(36)");

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

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("08db0f36-7dbb-436f-88e5-f1be70b3bda6"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "8e561f4f-5d33-4fbb-9fd1-838646ff61e6",
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FullName = "Admin",
                            IsMale = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAEJB64JV9rkRsMrR+BVLq8GX1bydA6+4DkyhU335E7oK2TQ+tF5NuXjDqJX4ijYfS+A==",
                            PhoneNumberConfirmed = false,
                            RoleId = new Guid("08db1e18-c46f-4e76-8e77-69430f54d796"),
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
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

                    b.HasOne("ESM.Data.Models.Shift", "Shift")
                        .WithMany("CandidateShift")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candidate");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ESM.Data.Models.Department", b =>
                {
                    b.HasOne("ESM.Data.Models.Faculty", "Faculty")
                        .WithMany("Departments")
                        .HasForeignKey("FacultyId");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("ESM.Data.Models.DepartmentShiftGroup", b =>
                {
                    b.HasOne("ESM.Data.Models.Department", "Department")
                        .WithMany("DepartmentShiftGroups")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("ESM.Data.Models.FacultyShiftGroup", "FacultyShiftGroup")
                        .WithMany("DepartmentShiftGroups")
                        .HasForeignKey("FacultyShiftGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Department");

                    b.Navigation("FacultyShiftGroup");

                    b.Navigation("User");
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

            modelBuilder.Entity("ESM.Data.Models.ExaminationData", b =>
                {
                    b.HasOne("ESM.Data.Models.Examination", "Examination")
                        .WithMany()
                        .HasForeignKey("ExaminationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Examination");
                });

            modelBuilder.Entity("ESM.Data.Models.FacultyShiftGroup", b =>
                {
                    b.HasOne("ESM.Data.Models.Faculty", "Faculty")
                        .WithMany("FacultyShiftGroups")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.ShiftGroup", "ShiftGroup")
                        .WithMany("FacultyShiftGroups")
                        .HasForeignKey("ShiftGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");

                    b.Navigation("ShiftGroup");
                });

            modelBuilder.Entity("ESM.Data.Models.InvigilatorShift", b =>
                {
                    b.HasOne("ESM.Data.Models.User", "CreatedBy")
                        .WithMany("CreatorInvigilatorShift")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Shift", "Shift")
                        .WithMany("InvigilatorShift")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ESM.Data.Models.Module", b =>
                {
                    b.HasOne("ESM.Data.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.HasOne("ESM.Data.Models.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("ESM.Data.Models.Shift", b =>
                {
                    b.HasOne("ESM.Data.Models.Room", "Room")
                        .WithMany("Shift")
                        .HasForeignKey("RoomId");

                    b.HasOne("ESM.Data.Models.ShiftGroup", "ShiftGroup")
                        .WithMany("Shifts")
                        .HasForeignKey("ShiftGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");

                    b.Navigation("ShiftGroup");
                });

            modelBuilder.Entity("ESM.Data.Models.ShiftGroup", b =>
                {
                    b.HasOne("ESM.Data.Models.Examination", "Examination")
                        .WithMany("ShiftGroups")
                        .HasForeignKey("ExaminationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ESM.Data.Models.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleId");

                    b.Navigation("Examination");

                    b.Navigation("Module");
                });

            modelBuilder.Entity("ESM.Data.Models.User", b =>
                {
                    b.HasOne("ESM.Data.Models.Department", "Department")
                        .WithMany("Users")
                        .HasForeignKey("DepartmentId");

                    b.HasOne("ESM.Data.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Role");
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

            modelBuilder.Entity("ESM.Data.Models.Candidate", b =>
                {
                    b.Navigation("CandidateShift");

                    b.Navigation("ExaminationModules");
                });

            modelBuilder.Entity("ESM.Data.Models.Department", b =>
                {
                    b.Navigation("DepartmentShiftGroups");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ESM.Data.Models.Examination", b =>
                {
                    b.Navigation("CandidatesOfModule");

                    b.Navigation("ShiftGroups");
                });

            modelBuilder.Entity("ESM.Data.Models.Faculty", b =>
                {
                    b.Navigation("Departments");

                    b.Navigation("FacultyShiftGroups");
                });

            modelBuilder.Entity("ESM.Data.Models.FacultyShiftGroup", b =>
                {
                    b.Navigation("DepartmentShiftGroups");
                });

            modelBuilder.Entity("ESM.Data.Models.Module", b =>
                {
                    b.Navigation("CandidatesOfExamination");
                });

            modelBuilder.Entity("ESM.Data.Models.Room", b =>
                {
                    b.Navigation("Shift");
                });

            modelBuilder.Entity("ESM.Data.Models.Shift", b =>
                {
                    b.Navigation("CandidateShift");

                    b.Navigation("InvigilatorShift");
                });

            modelBuilder.Entity("ESM.Data.Models.ShiftGroup", b =>
                {
                    b.Navigation("FacultyShiftGroups");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("ESM.Data.Models.User", b =>
                {
                    b.Navigation("CreatorInvigilatorShift");

                    b.Navigation("Examinations");
                });
#pragma warning restore 612, 618
        }
    }
}
