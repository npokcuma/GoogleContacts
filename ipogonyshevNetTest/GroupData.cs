﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipogonyshevNetTest
{
    internal class GroupData
    {
        private string groupName;
        private string groupId;
        private int? groupMemberCount;

        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
            }
        }
        public string GroupId
        {
            get { return groupId; }
            set
            {
                groupId = value;
            }
        }
        public  int? GroupMemberCount
        {
            get { return groupMemberCount; }
            set
            {
                groupMemberCount = value;
            }
        }
    }
}

