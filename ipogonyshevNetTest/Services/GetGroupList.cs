﻿using System.Collections.Generic;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;

namespace ipogonyshevNetTest.Services
{
	internal class GetGroupList:GroupData
	{
		public static List<GroupData> Getlist(PeopleServiceService service)
		{

			ContactGroupsResource groupsResource = new ContactGroupsResource(service);
			ContactGroupsResource.ListRequest listRequest = groupsResource.List();
			ListContactGroupsResponse response = listRequest.Execute();

			//eg to show name of each group
			List<GroupData> groupNames = new List<GroupData>();
			foreach (ContactGroup group in response.ContactGroups)
			{
				groupNames.Add(new GroupData() { GroupName = group.FormattedName,GroupId=group.ResourceName,GroupMemberCount=group.MemberCount });
			}
			return groupNames;
		}
	}
}

