using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace BigBadBlog.PostService.Api;

public static class PostEndpoints
{
    public static void MapPostEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Post").WithTags(nameof(Post));

        group.MapGet("/", async (PostDbContext db) =>
        {
            return await db.Posts.ToListAsync();
        })
        .WithName("GetAllPosts")
        .WithOpenApi();

        group.MapGet("/{slug}", async Task<Results<Ok<Post>, NotFound>> (string slug, PostDbContext db) =>
        {
            return await db.Posts.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Slug == slug)
                is Post model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPostBySlug")
        .WithOpenApi();

        group.MapPut("/{slug}", async Task<Results<Ok, NotFound>> (string slug, Post post, PostDbContext db) =>
        {
            var affected = await db.Posts
                .Where(model => model.Slug == slug)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Slug, post.Slug)
                    .SetProperty(m => m.Title, post.Title)
                    .SetProperty(m => m.Author, post.Author)
                    .SetProperty(m => m.Date, post.Date)
                    .SetProperty(m => m.Content, post.Content)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePost")
        .WithOpenApi();

        group.MapPost("/", async (Post post, PostDbContext db) =>
        {
            db.Posts.Add(post);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Post/{post.Slug}",post);
        })
        .WithName("CreatePost")
        .WithOpenApi();

        group.MapDelete("/{slug}", async Task<Results<Ok, NotFound>> (string slug, PostDbContext db) =>
        {
            var affected = await db.Posts
                .Where(model => model.Slug == slug)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePost")
        .WithOpenApi();
    }
}
