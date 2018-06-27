namespace PayItForward.Web.Models.ManageViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PayItForward.Common;

    public class ManageRolesViewModel
    {
        public bool IsAdmin { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Role
        {
            get
            {
                if (this.IsAdmin)
                {
                    return GlobalConstants.AdminRole;
                }

                return GlobalConstants.UserRole;
            }
        }
    }
}
