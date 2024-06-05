using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BigBadBlog.PostService.Api;

public class PostDbContext : DbContext
{
	public PostDbContext(DbContextOptions<PostDbContext> options) : base(options)
	{
	}

	public static PostDbContext Create(IMongoDatabase database) =>
		new (new DbContextOptionsBuilder<PostDbContext>()
										.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
										.Options);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Post>()
			.ToCollection("posts");
	}

	public DbSet<Post> Posts { get; set; }


}