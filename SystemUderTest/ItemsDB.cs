using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SystemUderTest;

public class ItemsDB : DbContext
{
    public ItemsDB(DbContextOptions<ItemsDB> options)
    : base(options) { }

    public DbSet<Item> Items => Set<Item>();
}

