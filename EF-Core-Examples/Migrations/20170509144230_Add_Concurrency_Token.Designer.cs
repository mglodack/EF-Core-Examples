using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EF_Core_Examples;

namespace EF_Core_Examples.Migrations
{
    [DbContext(typeof(AppContext))]
    [Migration("20170509144230_Add_Concurrency_Token")]
    partial class Add_Concurrency_Token
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("EF_Core_Examples.Folder", b =>
                {
                    b.Property<Guid>("Id")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Folder");
                });
        }
    }
}
