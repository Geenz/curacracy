using System;
using System.Collections;
using System.Collections.Generic;

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
    
    public class GenericRequest {
        public int authid {get; set;}
        public string authtoken {get; set;}
    }
    
    public class UserRequest : GenericRequest {
		
		public UserRequest() {
			
		}
        
        public UserRequest(UserResponse response) {
            this.userId = response.userId;
            this.username = response.username;
            this.registrationDate = response.registrationDate;
			this.rank = response.rank;
			this.description = response.description;
			this.shoutThreadId = response.shoutThreadId;
			this.rootFolders = response.rootFolders;
        }
		
		public UserRequest(int userId, string username, DateTime registrationDate, int rank, string description, long shoutThreadId, ICollection<Folder> rootFolders) {
			this.userId = userId;
			this.username = username;
			this.registrationDate = registrationDate;
			this.rank = rank;
			this.description = description;
			this.shoutThreadId = shoutThreadId;
			this.rootFolders = rootFolders;
		}
		
		public UserRequest(UserMeta metadata) {
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
		
		public long shoutThreadId {get; set;}
		
		public ICollection<Folder> rootFolders {get; set;}
    }
}