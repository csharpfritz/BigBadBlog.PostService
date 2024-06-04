var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddMongoDB("db")
	.WithMongoExpress()
	.AddDatabase("posts-database");

var api = builder.AddProject<Projects.BigBadBlog_PostService_Api>("api")
	.WithReference(db);

builder.Build().Run();
