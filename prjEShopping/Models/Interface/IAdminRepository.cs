using prjEShopping.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.Interface
{
    public interface IAdminRepository
    {
        IEnumerable<AdminDto> Search();
    }
}