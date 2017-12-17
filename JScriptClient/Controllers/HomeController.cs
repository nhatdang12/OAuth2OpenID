using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JScriptClient.Controllers
{
    public class HomeController: Controller
    {
        private readonly ClientConfiguration _clientConfig;

        public HomeController(IOptions<ClientConfiguration> clientConfigOptions)
        {
            _clientConfig = clientConfigOptions?.Value;
        }

        public IActionResult Index()
        {
            return Json(_clientConfig);
        }
    }
}
