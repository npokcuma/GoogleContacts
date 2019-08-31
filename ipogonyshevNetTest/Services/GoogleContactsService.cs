using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	internal class GoogleContactsService : IContactService
	{
		private static PeopleServiceService _service;
		private List<Contact> _listContacts;
		private List<Label> _listLabels;


		public GoogleContactsService()
		{
			_service = Authorization.GetGooglePeopleService();
		}

		private void ReloadContacts()
		{
			var groupRequest = new ContactGroupsResource(_service).List();
			var response = groupRequest.Execute();

			var allGroups = response.ContactGroups.Select(group => new Label
			{
				Id = group.ResourceName,
				Name = group.FormattedName
			}).ToList();
			_listLabels = allGroups;

			var peopleRequest = _service.People.Connections.List("people/me");
			peopleRequest.PersonFields = "addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders,imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers,photos,relations,relationshipInterests,relationshipStatuses,residences,sipAddresses,skills,taglines,urls,userDefined";
			peopleRequest.SortOrder = (PeopleResource.ConnectionsResource.ListRequest.SortOrderEnum)1;
			peopleRequest.PageSize = 1000;
			var response1 = peopleRequest.Execute();

			_listContacts = new List<Contact>();
			foreach (var person in response1.Connections)
			{
				var contact = new Contact
				{
					Name = person.Names?[0]?.DisplayName,
					PhoneNumber = Convert.ToString(person.PhoneNumbers?[0]?.CanonicalForm),
					EmailAddress = Convert.ToString(person.EmailAddresses?[0]?.Value),
					Id = person.ResourceName
				};
				_listContacts.Add(contact);

				foreach (var membership in person.Memberships)
				{
					_listLabels.First(l => l.Id == membership.ContactGroupMembership.ContactGroupResourceName).Contacts.Add(contact);
				}
			}
		}

		public List<Contact> GetAllContacts()
		{
			ReloadContacts();
			return _listContacts;
		}

		public bool DeleteContact(Contact contact)
		{
			return true;
		}

		public bool CreateContact(Contact contact)
		{
			return true;
		}

		public bool UpdateContact(Contact contact)
		{


			return true;
		}

		public List<Label> GetAllLabels()
		{
			ReloadContacts();
			return _listLabels;
		}

		public bool CreateLabel(Label label)
		{
			return true;
		}

		public bool RenameLabel(Label label)
		{
			return true;
		}

		public bool DeleteLabel(Label label)
		{
			return true;
		}

		public bool UpdateLabel(Label label)
		{
			return true;
		}
	}
}
