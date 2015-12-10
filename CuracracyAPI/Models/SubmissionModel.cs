using System;
using System.Collections;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace CuracracyAPI.Models {
	public class SubmissionMeta {
		public long id {get; set;}
		public int userId {get; set;}
		public int type {get; set;}
		public string title {get; set;}
		public int rating {get; set;}
		public DateTime posted {get; set;}
		public string thumbnailUrl {get; set;}
	}
	
	public class Submission {
		public long submissionId {get; set;}
		
		public long mediaId {get; set;}
		
		public long threadId {get; set;}
		
		public string description {get; set;}
	}
	
	public class Media {
		public long id {get; set;}
		public string mediaUrl {get; set;}
		public int mediaType {get; set;}
	}
}