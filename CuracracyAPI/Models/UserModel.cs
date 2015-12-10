using System;
using System.Collections;
using Microsoft.AspNet.Mvc;

namespace CuracracyAPI.Models {
	public class UserMeta {
		public int id {get; set;}
		
		public DateTime registrationDate {get; set;}
		
		public DateTime birthDate {get; set;}
		
		public string userName {get; set;}
		
		public string email {get; set;}
		
		public int rank {get; set;}
	}
	
	public class User {
		public int userId {get; set;}
		
		public long galleryFolderId {get; set;}
		
		public long shoutsThreadId {get; set;}
		
		public string description {get; set;}
	}
}