using System;
using System.Collections;
using System.Collections.Generic;

namespace CuracracyAPI.Models {
	public class LoginResponse {
		public LoginResponse(string session, int id) {
			this.token = session;
			this.userid = id;
		}
		
		public string token {get; set;}
		
		public int userid {get; set;}
	}
	
	public class ValidationResponse {
		public ValidationResponse(bool validated, string reason) {
			this.validated = validated;
			this.reason = reason;
		}
		
		public bool validated {get; set;}
		
		public string reason {get; set;}
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