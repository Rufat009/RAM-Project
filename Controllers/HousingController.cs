using Microsoft.AspNetCore.Mvc;
using project.Models;
using project.Services;
using System;
using System.Linq;

namespace project.Controllers
{
    public class HousingController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Balance = BalanceManager.GetBalance();
            var allChecks = CheckService.GetAllChecks();
            ViewBag.Checks = allChecks.Where(c => c.Service == "Housing").OrderByDescending(c => c.TimeBought).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Buy(PurchaseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ItemName) || request.Price <= 0)
            {
                ModelState.AddModelError("", "Invalid input.");
                ViewBag.Balance = BalanceManager.GetBalance();
                return View("Index");
            }
            if (!BalanceManager.TryDeduct(request.Price))
            {
                ModelState.AddModelError("", "Insufficient balance.");
                ViewBag.Balance = BalanceManager.GetBalance();
                return View("Index");
            }
            var check = new Check
            {
                Service = "Housing",
                ItemName = request.ItemName,
                AmountDeducted = request.Price,
                BalanceAfter = BalanceManager.GetBalance(),
                TimeBought = DateTime.UtcNow
            };
            CheckService.AddCheck(check);
            return RedirectToAction("Success", new { id = Guid.NewGuid(), item = check.ItemName, amount = check.AmountDeducted, balance = check.BalanceAfter, time = check.TimeBought });
        }

        [HttpGet]
        public IActionResult Success(string item, decimal amount, decimal balance, DateTime time)
        {
            ViewBag.Item = item;
            ViewBag.Amount = amount;
            ViewBag.Balance = balance;
            ViewBag.Time = time;
            return View();
        }
    }
} 