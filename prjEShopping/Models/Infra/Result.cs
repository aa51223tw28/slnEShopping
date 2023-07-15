using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.Infra
{
    public class Result
    {
        public bool IsSuccess { get;private set; }//private不能被改變value
        public bool IsFail=>!IsSuccess;
        public string ErrorMessage { get; private set; }
        public static Result Success()=>new Result { IsSuccess = true, ErrorMessage = null };//static方法可以不要new
        public static Result Fail(string errorMessage) => new Result { IsSuccess = false, ErrorMessage = errorMessage };
    }
}