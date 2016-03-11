﻿using System.Runtime.Serialization;
using Thalia.Services.Photos.Api500px.Intefaces;

namespace Thalia.Services.Photos.Api500px.Contracts
{
	[DataContract]
	public abstract class PagedResponse: Response, IPagedResponse
	{
		#region Public Properties
		[DataMember(Name = "current_page")]
		public virtual int CurrentPage { get; set; }

		[DataMember(Name = "total_items")]
		public virtual int TotalItems { get; set; }

		[DataMember(Name = "total_pages")]
		public virtual int TotalPages { get; set; }
		#endregion
	}
}