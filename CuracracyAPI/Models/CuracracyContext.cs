using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Entity;
using Microsoft.Extensions.PlatformAbstractions;

namespace CuracracyAPI.Models {
	public class CuracracyContext : DbContext {
		public DbSet<SubmissionMeta> SubmissionMetadata {get; set;}
		
		public DbSet<Submission> Submissions {get; set;}
		
		public DbSet<Media> MediaMetadata {get; set;}
		
		public DbSet<UserMeta> UserMetadata {get; set;}
		
		public DbSet<User> Users {get; set;}
		
		public DbSet<ThreadMeta> ThreadMetadata {get; set;}
		
		public DbSet<ThreadComment> ThreadComments {get; set;}
		
		public DbSet<Folder> Folders {get; set;}
		
		public DbSet<FolderEntry> FolderEntries {get; set;}
		
		public DbSet<AuthenticatedSession> AuthenticatedSessions {get; set;}
		
		public DbSet<AuthenticatedUser> AuthenticatedUsers {get; set;}
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            optionsBuilder.UseSqlite("Filename=" + Path.Combine(path, "curacracy.db"));
        }
	}
}
