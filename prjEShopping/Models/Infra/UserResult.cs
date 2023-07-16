using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.Infra
{
    public class UserResult
    {
        public bool IsSuccess { get;private set; }//private不能被改變value
        public bool IsFail=>!IsSuccess;
        public string ErrorMessage { get; private set; }
        public static UserResult Success()=>new UserResult { IsSuccess = true, ErrorMessage = null };//static方法可以不要new
        public static UserResult Fail(string errorMessage) => new UserResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}