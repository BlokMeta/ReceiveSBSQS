using Microsoft.AspNetCore.Mvc;
using ReceiveMessagesSQS_SB.Services;
using ReceiveSBSQS.Models;
using System.Diagnostics;

namespace ReceiveSBSQS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReceivers _receivers;


        public HomeController(
            IReceivers Receivers)
        {
            _receivers = Receivers;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReceiveMessageAsync()
        {
            ReceiveQueueModel model;
            model = await _receivers.ReceiveMessageAsync();
            return View(model);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}