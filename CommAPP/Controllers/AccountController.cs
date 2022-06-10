using Comm.Business.Abstract;
using Comm.DataAccess;
using Comm.Entities;
using CommAPP.Models;
using CommAPP.Models.ViewModels;
using CommAPP.Models.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommAPP.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;


        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailSender emailSender, ICartService cartService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cartService = cartService;


        }


        public IActionResult Login(string ReturnUrl = null)
        {

            return View(new LoginModel { ReturnUrl = ReturnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Belə bir email mövcud deyil");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Zəhmət olmasa hesabınızı təsdiqləyin");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (result.Succeeded)
            {
                if (SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart") != null)
                {
                    //Pass items in session to User Cart after login completed
                    await PassSessionToUserCart(user.Id);

                    return RedirectToAction("Index", "Home");

                }

                return Redirect(model.ReturnUrl ?? "~/");
            }



            ModelState.AddModelError("", "Daxil edilən email və ya parol yanlışdır");
            return View(model);
        }

        public async Task PassSessionToUserCart(string userId)
        {
            if (SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart") != null)
            {
                CartModel cartModel = SessionHelper.GetObjectFromJson<CartModel>(HttpContext.Session, "cart");
                var cart  =await _cartService.GetCartByUserIdAsync(userId); 

                if (cart != null)
                {
                    foreach (var item in cartModel.CartItems)
                    {

                        var index = cart.CartItems.FindIndex(i => i.ProductId == item.ProductId);
                        if (index < 0)
                        {
                            cart.CartItems.Add(new CartItem() { CartId = cart.Id, ProductId = item.ProductId, Quantity = item.Quantity });
                        }
                        else
                        {
                            cart.CartItems[index].Quantity += item.Quantity;
                        }


                    }

                    await _cartService.Update(cart);

                }


            }

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Register(RegisterModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser()

                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email

                };

                var result = await _userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userId = appUser.Id,
                        token = code
                    });

                    await _emailSender.SendEmailAsync(model.Email, "E-mail'i təsdiqləyin", $"Hesabınızı təsdiqləmək üçün <a href='https://localhost:44390{confirmationLink}'>klikləyin</a>");


                    return RedirectToAction("SuccessRegistration", "Account");
                }
            }


            ModelState.AddModelError("", "Daxil edilən email yanlışdır");
            return View(model);


        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                //After confirmation create initial cart for user
                await _cartService.InitializeCartAsync(user.Id);
                HttpContext.Session.Clear();



                return RedirectToAction("Index", "Cart");



            }

            return NotFound();
        }


        public IActionResult SuccessRegistration()
        {
            return View();
        }


        public IActionResult SuccessReset()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Belə bir email yoxdur");
                return View(model);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            await _emailSender.SendEmailAsync(model.Email, "Reset Password", $"Reset password <a href='{callback}'>klikləyin</a>");


            return RedirectToAction("SuccessReset", "Account");
        }


        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Belə bir istifadəçi yoxdur");
                return View(model);
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPassResult.Succeeded)
            {
                ModelState.AddModelError("", "Bilinməyən xıta başverdi.. Bir daha yoxlayın...");
                return View(model);
            }

            return RedirectToAction("ResetPasswordConfirmation", "Account");

        }



        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
                // return RedirectToAction("Login");
            }
            var model = new ProfileDetailsModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };


            return View(model);
        }
        public async Task<IActionResult> ProfileEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Login");
            }
            var model = new ProfileDetailsModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileEdit(ProfileDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Profile", new { id = model.Id });
                }

            }
            return View(model);
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}


