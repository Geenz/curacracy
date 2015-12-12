using System;
using System.Collections;
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
}