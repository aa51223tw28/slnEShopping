using prjEShopping.Models.DTOs;
using prjEShopping.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.Services
{
    public class AdminService
    {
        private IAdminRepository _repo;

        public AdminService(IAdminRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<AdminDto> Search()
        {
            return _repo.Search();
        }
    }
}