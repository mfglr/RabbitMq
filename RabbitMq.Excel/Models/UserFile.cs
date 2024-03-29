﻿namespace RabbitMq.Excel.Models
{


	public class UserFile
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public string FileName { get; set; }
		public string? FilePath { get; set; }
		public DateTime? CreatedDate { get; set; }
		public FileState FileState { get; set; }

	}

	public enum FileState
	{
		Creating,
		Completed
	}

}
