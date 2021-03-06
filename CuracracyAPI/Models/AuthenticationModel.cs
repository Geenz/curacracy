using System;
using System.Collections;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace CuracracyAPI.Models {
	public class AuthenticatedSession {
		public long id {get; set;}
		
		public string sessionId { get; set;}
		
		public DateTime expirationDate {get; set;}
		
		public virtual UserMeta user {get; set;}
	}
	
	public class AuthenticatedUser {
		public int id {get; set;}
		
		public string password {
			get {
				return _password;
			} 
			set {
				_password = BCrypt.Net.BCrypt.HashPassword(value, salt);
			}
		}
		
		public string salt {get; set;}
		
		private string _password;
		
		public string hashMethod {get; set;}
		
		public string email {get; set;}
		
		public virtual UserMeta user {get; set;}
	}
}
