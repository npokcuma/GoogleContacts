using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;

namespace ipogonyshevNetTest
{
    class GetContactsList:ContactsData
    {
        public static List<ContactsData> GetList(PeopleServiceService service)
        {
            // Contact list ////////////
            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
                service.People.Connections.List("people/me");
            peopleRequest.PersonFields = "addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders,imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers,photos,relations,relationshipInterests,relationshipStatuses,residences,sipAddresses,skills,taglines,urls,userDefined";
            peopleRequest.SortOrder = (PeopleResource.ConnectionsResource.ListRequest.SortOrderEnum)1;
            ListConnectionsResponse people = peopleRequest.Execute();

            // eg to show display name of each contact
            List<ContactsData> contacts = new List<ContactsData>();
            foreach (var person in people.Connections)
            {
                contacts.Add(new ContactsData() { Name = person.Names[0].DisplayName, PhoneNumber = Convert.ToString(person.PhoneNumbers[0].CanonicalForm), EmailAddress = Convert.ToString(person.EmailAddresses[0].Value), ContactId = person.ResourceName }) ;
            }
            return contacts;
        }
}
}
