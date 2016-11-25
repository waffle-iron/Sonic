using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Sonic.Domain.Concrete;

namespace Sonic.Domain.Migrations
{
    [DbContext(typeof(SonicDbContext))]
    [Migration("20161125175540_AddMethods")]
    partial class AddMethods
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Sonic.Domain.Entities.Method", b =>
                {
                    b.Property<int>("MethodId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SystemId");

                    b.HasKey("MethodId");

                    b.HasIndex("SystemId");

                    b.ToTable("Methods");
                });

            modelBuilder.Entity("Sonic.Domain.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SystemId");

                    b.HasKey("RoleId");

                    b.HasIndex("SystemId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Sonic.Domain.Entities.System", b =>
                {
                    b.Property<int>("SystemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("SystemId");

                    b.ToTable("Systems");
                });

            modelBuilder.Entity("Sonic.Domain.Entities.Method", b =>
                {
                    b.HasOne("Sonic.Domain.Entities.System", "System")
                        .WithMany("Methods")
                        .HasForeignKey("SystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Sonic.Domain.Entities.Role", b =>
                {
                    b.HasOne("Sonic.Domain.Entities.System", "System")
                        .WithMany("Roles")
                        .HasForeignKey("SystemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
