﻿namespace SV21T1020547.DomainModels
{
    public class UserAccount
    {
        public string UserID { get; set; } = "";
        public string UserName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Photo { get; set; } = "";
        public string RoleNames { get; set; } = "";
        public bool Active { get; set; } = true;
    }
}
