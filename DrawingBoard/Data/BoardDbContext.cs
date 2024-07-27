﻿using DrawingBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace DrawingBoard.Data;

public class BoardDbContext : DbContext
{
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
    {
        //Database.Migrate();
    }

    public DbSet<Board> Boards { get; set; }
    public DbSet<DrawingElement> DrawingElements { get; set; }
}