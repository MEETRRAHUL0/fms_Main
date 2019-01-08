using FMS.API;
using FMS.Models;
using System.Collections.Generic;
using System.Linq;

namespace FMS.Helper
{
    public static class MenuHelper
    {
        //private readonly static List<Menu> _menu;

        public static MenuEntity LoadMenu()
        {
            MenuEntity MenuResponce = new MenuEntity();

            MenusAPIController menusAPIController = new MenusAPIController();

            var _menu = menusAPIController.GetMenus().Where(q => q.IsEnable != false).OrderBy(o => o.ordinal).ToList();

            var menuItems = GenerateTree(_menu, _menu.Where(q => q.ParentID == -1).First());

            MenuResponce.menuItems = menuItems.ToList();
            MenuResponce.associateId = "Test";


            return MenuResponce;
        }

        public static IList<MenuItem> GenerateTree(IEnumerable<tblMenu> collection, tblMenu rootItem)
        {
            IList<MenuItem> lst = new List<MenuItem>();


            foreach (tblMenu c in collection.Where(c => c.ParentID == rootItem.ID))
            {
                lst.Add(new MenuItem
                {
                    displayName = c.DisplayName,
                    icon = c.icon,
                    ordinal = c.ordinal,
                    path = c.path,
                    type = c.type,
                    childItems = GenerateTree(collection.ToList(), c).ToList()
                });
            }
            return lst;
        }
    }
}