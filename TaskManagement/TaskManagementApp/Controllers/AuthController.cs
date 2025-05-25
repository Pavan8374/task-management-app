using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Auth;
using TaskManagement.Application.Interfaces;
using TaskManagementApp.Common;

namespace TaskManagementApp.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var response = await _authService.LoginAsync(request);

                // Store token in session or cookie as needed
                HttpContext.Session.SetString("AuthToken", response.Token);
                HttpContext.Session.SetString("UserEmail", response.Email);
                HttpContext.Session.SetString("UserId", response.UserId.ToString());
                HttpContext.Session.SetString("UserRole", response.Role);

                TempData["SuccessMessage"] = "Login successful!";

                return response.Role == "Admin" ? RedirectToAction("Index", "Admin") : RedirectToAction("Dashboard", "Task");

            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View(request);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var response = await _authService.SignUpAsync(request);

                // Store token in session or cookie as needed
                HttpContext.Session.SetString("AuthToken", response.Token);
                HttpContext.Session.SetString("UserEmail", response.Email);
                HttpContext.Session.SetString("UserId", response.UserId.ToString());
                HttpContext.Session.SetString("UserRole", response.Role ?? "User");

                TempData["SuccessMessage"] = "Registration successful! Welcome to Task Management.";

                return response.Role == "Admin" ? RedirectToAction("Index", "Admin") : RedirectToAction("Dashboard", "Task");

            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View(request);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
