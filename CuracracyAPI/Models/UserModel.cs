using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;

namespace CuracracyAPI.Models {
	public class UserMeta {
		public int id {get; set;}
		
		public DateTime registrationDate {get; set;}
		
		public DateTime birthDate {get; set;}
		
		public string userName {get; set;}
		
		public int rank {get; set;}
		
		public virtual User userdata {get; set;}
	}
	
	public class User {
		public int id {get; set;}
		public virtual UserMeta metadata {get; set;}
		
		public Folder galleryFolder {get; set;}
		
		public long shoutsThreadId {get; set;}
		
		public string description {get; set;}
	}
	
	public class UserResponse {
		
		public UserResponse(UserMeta metadata) {
			this.userId = metadata.id;
			this.username = metadata.userName;
			this.registrationDate = metadata.registrationDate;
			this.rank = metadata.rank;
			this.description = metadata.userdata.description;
			this.shoutThreadId = metadata.userdata.shoutsThreadId;
			this.rootFolders = metadata.userdata.galleryFolder.children;
		}
		
		public int userId {get; set;}
		
		public string username {get; set;}
		
		public DateTime registrationDate {get; set;}
		
		public int rank {get; set;}
		
		public string description {get; set;}
		
		public long shoutThreadId;
		
		public ICollection<Folder> rootFolders;
	}
	
	public class GenericReponse {
		public GenericReponse(bool success, string message) {
			this.success = success;
			this.message = message;
		}
		
		public bool success {get; set;}
		
		public string message {get; set;}
	}
}