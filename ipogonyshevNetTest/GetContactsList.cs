using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System;
using System.Collections.Generic;

namespace ipogonyshevNetTest
{
	class GetContactsList
	{
		public static List<Contact> GetList(PeopleServiceService service)
		{
			// Contact list ////////////
			PeopleResource.ConnectionsResource.ListRequest peopleRequest =
				service.People.Connections.List("people/me");
			peopleRequest.PersonFields = "addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders,imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers,photos,relations,relationshipInterests,relationshipStatuses,residences,sipAddresses,skills,taglines,urls,userDefined";
			peopleRequest.SortOrder = (PeopleResource.ConnectionsResource.ListRequest.SortOrderEnum)1;
			ListConnectionsResponse people = peopleRequest.Execute();

			// eg to show display name of each contact
			List<Contact> contacts = new List<Contact>();
			foreach (var person in people.Connections)
			{
				try
				{
					contacts.Add(new Contact()
					{
						Name = person.Names[0].DisplayName,
						PhoneNumber = Convert.ToString(person.PhoneNumbers[0].CanonicalForm),
						EmailAddress = Convert.ToString(person.EmailAddresses[0].Value), Id = person.ResourceName
					});
				}
				catch  (Exception ex)
				{
					
				}
			}
			return contacts;
		}
	}
}
