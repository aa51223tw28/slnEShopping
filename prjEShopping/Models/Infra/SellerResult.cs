using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.Infra
{
    public class SellerResult
    {
        public bool IsSuccess { get; private set; }//private不能被改變value
        public bool IsFail => !IsSuccess;
        public string ErrorMessage { get; private set; }
        public static SellerResult Success() => new SellerResult { IsSuccess = true, ErrorMessage = null };//static方法可以不要new
        public static SellerResult Fail(string errorMessage) => new SellerResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}