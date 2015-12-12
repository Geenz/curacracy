using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;

namespace CuracracyAPI.Models {
	public class Folder {
		public long id {get; set;}
		public int permissions {get; set;}
		public string name {get; set;}
		
		public virtual ICollection<Folder> children {get; set;}
	}
	
	public class FolderEntry {
		public long id {get; set;}
		
		public long submissionId {get; set;}
		
		public long folderId {get; set;}
	}
}
