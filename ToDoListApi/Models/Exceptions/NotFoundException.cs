using System;

namespace ToDoListApi.Models.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string? message) : base(message)
		{
		}
	}
}
