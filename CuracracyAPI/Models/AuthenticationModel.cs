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
	
	public class LoginResponse {
		public LoginResponse(AuthenticatedSession session) {
			if (session != null) {
				// We basically handle null as a failure case.  Something went wrong.
				this.token = session.sessionId;
				this.userid = session.user.id;
			}
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
}
