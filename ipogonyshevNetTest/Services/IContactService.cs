using System.Collections.Generic;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	/// <summary>
	/// Interface for working with contacts, groups.
	/// </summary>
	public interface IContactService
	{
		List<Contact> GetAllContacts();

		bool DeleteContact(Contact contact);

		/// <summary>
		/// Create new contact. 
		/// </summary>
		/// <param name="contact">New contact model</param>
		/// <returns>Return contact Id. If creation was not successful return null</returns>
		string CreateContact(Contact contact);

		bool UpdateContact(Contact contact);

		List<Label> GetAllLabels();

		string CreateLabel(Label label);

		bool DeleteLabel(Label label);

		bool UpdateLabel(Label label);

		bool AddLabelToContact(Contact contact, Label label);

		bool RemoveLabelFromContact(Contact contact, Label label);
	}
}
