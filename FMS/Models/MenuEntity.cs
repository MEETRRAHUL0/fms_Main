using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Models
{
    public class Menu
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string icon { get; set; }
        public string type { get; set; }
        public int ordinal { get; set; }
        public string path { get; set; }
        public int ParentID { get; set; }
        public bool? IsEnable { get; set; }
    }
    public class MenuEntity
    {
        public string createdDateTime { get; set; }
        public string deviceType { get; set; }
        public string role { get; set; }
        public string associateId { get; set; }
        public List<MenuItem> menuItems { get; set; }
    }


    public class MenuItem
    {
        public List<MenuItem> childItems { get; set; }
        public string displayName { get; set; }
        public string icon { get; set; }
        public string type { get; set; }
        public int ordinal { get; set; }
        public string path { get; set; }
    }


    public class MenuInputs
    {
        public string DeviceType { get; set; }
        public int Division { get; set; }
        public int Location { get; set; }  // Store
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string AssociateId { get; set; }
    }
}