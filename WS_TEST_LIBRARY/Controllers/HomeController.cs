using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WS_TEST_LIBRARY.Interfaces;
using WS_TEST_LIBRARY.Models;

namespace WS_TEST_LIBRARY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICoordinatesService _coordinatesService;

        public HomeController(ILogger<HomeController> logger, ICoordinatesService coordinatesService)
        {
            _logger = logger;
            _coordinatesService = coordinatesService;
        }

        public IActionResult Index()
        {
            var model = new CoordinatesModel();
            model.Results = new List<RoverResultModel>();
            model.Results.Add(new RoverResultModel());
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CoordinatesModel model)
        {
            var currentRover = model.Results[model.Results.Count - 1];

            try
            {
                _coordinatesService.SetEnd(model.MaxCoordinates);
                _coordinatesService.SetCurrentPosition(currentRover.CurrentPosition);
                currentRover.Result = _coordinatesService.Calculate(currentRover.Movement);
                model.Results.Add(new RoverResultModel());
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
            }

            return View(model);
        }
    }
}
