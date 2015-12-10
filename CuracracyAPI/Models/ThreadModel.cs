using System;
using System.Collections;
using Microsoft.AspNet.Mvc;

namespace CuracracyAPI.Models {
	public class ThreadMeta {
		public long id {get; set;}
		
		public string threadTopic {get; set;}
		
		public short threadStatus {get; set;}
	}
	
	public class ThreadComment {
		public long id {get; set;}
		public int commentThreadId { get; set;}
		
		public long commentParentId {get; set;}
		
		public string commentSubject {get; set;}
		
		public string commentBody {get; set;}
	}
}
