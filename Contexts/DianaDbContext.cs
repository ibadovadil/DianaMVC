﻿using Diana.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace Diana.Mvc.Contexts;
public class DianaDbContext : DbContext
{
    public DianaDbContext(DbContextOptions opt) : base(opt) { }

    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ProductColor> ProductColors { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }

}
