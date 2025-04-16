﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Moviesplatform.Controllers
{
    [Authorize(Roles = "User")]

    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
