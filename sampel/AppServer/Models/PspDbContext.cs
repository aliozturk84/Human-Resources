using AppServer.Models;
using Microsoft.EntityFrameworkCore;

public class PspDbContext(DbContextOptions<PspDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; } = default!;
}