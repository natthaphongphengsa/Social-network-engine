﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TDDInlämningsuppgift.Data;

namespace TDDInlämningsuppgift.Migrations
{
    [DbContext(typeof(ApplicationDb))]
    partial class ApplicationDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TDDInlämningsuppgift.Model.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("SendFromId")
                        .HasColumnType("int");

                    b.Property<int?>("SendToId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SendToId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("TDDInlämningsuppgift.Model.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Datum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PostedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostedById");

                    b.ToTable("Posters");
                });

            modelBuilder.Entity("TDDInlämningsuppgift.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<int>("FollowerId")
                        .HasColumnType("int");

                    b.Property<int>("FollowingId")
                        .HasColumnType("int");

                    b.HasKey("FollowerId", "FollowingId");

                    b.HasIndex("FollowingId");

                    b.ToTable("UserUser");
                });

            modelBuilder.Entity("TDDInlämningsuppgift.Model.Chat", b =>
                {
                    b.HasOne("TDDInlämningsuppgift.User", "SendTo")
                        .WithMany("Chats")
                        .HasForeignKey("SendToId");

                    b.Navigation("SendTo");
                });

            modelBuilder.Entity("TDDInlämningsuppgift.Model.Post", b =>
                {
                    b.HasOne("TDDInlämningsuppgift.User", "PostedBy")
                        .WithMany("posters")
                        .HasForeignKey("PostedById");

                    b.Navigation("PostedBy");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("TDDInlämningsuppgift.User", null)
                        .WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDDInlämningsuppgift.User", null)
                        .WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDDInlämningsuppgift.User", b =>
                {
                    b.Navigation("Chats");

                    b.Navigation("posters");
                });
#pragma warning restore 612, 618
        }
    }
}
