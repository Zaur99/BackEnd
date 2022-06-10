using Comm.Business.Abstract;
using Comm.DataAccess;
using Comm.Entities;
using CommAPP.Models;
using CommAPP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private ICartService _cartService;
        private UserManager<ApplicationUser> _userManager;

        public BasketViewComponent(ICartService cartService,UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int count;
            if (User.Identity.IsAuthenticated)
            {
                count =await _cartService.GetCountItemsAsync(_userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)User));
                if (count > 0)
                {
                    ViewData["Count"] = count;
                }
                else {
                    ViewData["Count"] = "";
                }
            }
            else
            {

                CartModel cartSession = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
                if (cartSession == null)
                {
                    ViewData["Count"] = ""; 
                }
                else
                {
                    int countForGuest=0;
                    foreach (var item in cartSession.CartItems)
                    {
                        countForGuest += item.Quantity;
                    }
                    ViewData["Count"] = countForGuest;
                }


            }

            return View();
        }
    }
}
