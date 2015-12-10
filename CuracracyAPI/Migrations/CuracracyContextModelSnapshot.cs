using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CuracracyAPI.Models;

namespace CuracracyAPI.Migrations
{
    [DbContext(typeof(CuracracyContext))]
    partial class CuracracyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("CuracracyAPI.Models.AuthenticatedSession", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("expirationDate");

                    b.Property<string>("sessionId");

                    b.Property<int>("userid");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.AuthenticatedUser", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("hashMethod");

                    b.Property<string>("password");

                    b.Property<string>("salt");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.Folder", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("name");

                    b.Property<long>("parentId");

                    b.Property<int>("permissions");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.FolderEntry", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("folderId");

                    b.Property<long>("submissionId");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.Media", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("mediaType");

                    b.Property<string>("mediaUrl");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.Submission", b =>
                {
                    b.Property<long>("submissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("description");

                    b.Property<long>("mediaId");

                    b.Property<long>("threadId");

                    b.HasKey("submissionId");
                });

            modelBuilder.Entity("CuracracyAPI.Models.SubmissionMeta", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("posted");

                    b.Property<int>("rating");

                    b.Property<string>("thumbnailUrl");

                    b.Property<string>("title");

                    b.Property<int>("type");

                    b.Property<int>("userId");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.ThreadComment", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("commentBody");

                    b.Property<long>("commentParentId");

                    b.Property<string>("commentSubject");

                    b.Property<int>("commentThreadId");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.ThreadMeta", b =>
                {
                    b.Property<long>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("threadStatus");

                    b.Property<string>("threadTopic");

                    b.HasKey("id");
                });

            modelBuilder.Entity("CuracracyAPI.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("description");

                    b.Property<long>("galleryFolderId");

                    b.Property<long>("shoutsThreadId");

                    b.HasKey("userId");
                });

            modelBuilder.Entity("CuracracyAPI.Models.UserMeta", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("birthDate");

                    b.Property<string>("email");

                    b.Property<int>("rank");

                    b.Property<DateTime>("registrationDate");

                    b.Property<string>("userName");

                    b.HasKey("id");
                });
        }
    }
}
