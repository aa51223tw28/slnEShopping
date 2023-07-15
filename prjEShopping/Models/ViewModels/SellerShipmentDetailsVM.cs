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

		public decimal Price { get; set; }

		public int Quantity { get; set; }

		public string ProductImagePathOne { get; set; }

		public decimal TotolPrice
		{
			get { return this.Price * this.Quantity  ; }
		}

		
	}
}