using System;
using System.Collections.Generic;

namespace ipogonyshevNetTest.Model
{
	public class Label
	{
		public Label()
		{
			Id = Guid.NewGuid().ToString();
		}


		public string Id { get; set; }

		public string Name { get; set; }

		public List<Contact> Contacts { get; set; } = new List<Contact>();
	}
}
