using prjEShopping.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace prjEShopping.Models
{
    public class CFactory
    {
        //public void create(SellerRegisterVM s)
        //{
        //    List<SqlParameter> paras = new List<SqlParameter>();

        //    string sql = "INSERT INTO Sellers(";
        //    if (!string.IsNullOrEmpty(s.SellerName))//檢視使用者可以不必填某些欄位
        //        sql += "SellerName,";
        //    if (!string.IsNullOrEmpty(s.Phone))
        //        sql += "Phone,";
        //    if (!string.IsNullOrEmpty(s.SellerAccount))
        //        sql += "SellerAccount,";
        //    if (!string.IsNullOrEmpty(s.Address))
        //        sql += "Address,";
        //    if (!string.IsNullOrEmpty(s.SellerPassword))
        //        sql += "SellerPassword,";
        //    判斷最後一碼如果是逗號要去除
        //    if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
        //        sql = sql.Trim().Substring(0, sql.Trim().Length - 1);


        //    sql += ")VALUES(";
        //    if (!string.IsNullOrEmpty(s.SellerName))
        //    {
        //        sql += "@K_SELLERNAME,";
        //        paras.Add(new SqlParameter("K_SELLERNAME", s.SellerName));
        //    }
        //    if (!string.IsNullOrEmpty(s.Phone))
        //    {
        //        sql += "@K_PHONE,";
        //        paras.Add(new SqlParameter("K_PHONE", s.Phone));
        //    }
        //    if (!string.IsNullOrEmpty(s.SellerAccount))
        //    {
        //        sql += "@K_SELLERACCOUNT,";
        //        paras.Add(new SqlParameter("K_SELLERACCOUNT", s.SellerAccount));
        //    }
        //    if (!string.IsNullOrEmpty(s.Address))
        //    {
        //        sql += "@K_ADDRESS,";
        //        paras.Add(new SqlParameter("K_ADDRESS", s.Address));
        //    }
        //    if (!string.IsNullOrEmpty(s.SellerPassword))
        //    {
        //        sql += "@K_SELLERPASSWORD,";
        //        paras.Add(new SqlParameter("K_SELLERPASSWORD", s.SellerPassword));
        //    }

        //    判斷最後一碼如果是逗號要去除且 +)
        //    if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
        //        sql = sql.Trim().Substring(0, sql.Trim().Length - 1);
        //    sql += ")";
        //    executeSql(sql, paras);
        //}

        //private void executeSql(string sql, List<SqlParameter> paras)
        //{
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = @"Data Source=.;Initial Catalog=EShopping;Integrated Security=True";
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(sql, con);
        //    if (paras != null)
        //        cmd.Parameters.AddRange(paras.ToArray());
        //    cmd.ExecuteNonQuery();
        //    con.Close();
        //}
    }
}