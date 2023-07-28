using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjEShopping.Models.ViewModels
{
	public class SellerShipmentDetailsVM
	{

		public int ProductId { get; set; }

		public string ProductName { get; set; }

		public int Price { get; set; }

		public int Quantity { get; set; }

		public string ProductImagePathOne { get; set; }

		public int ItemPrice
		{
			get { return (int)(this.Price * this.Quantity)  ; }
		}

		public int ChangedQty { get; set; }

		
	}
}