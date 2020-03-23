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

        private readonly IList<ReactRoverResultModel> _rovers;

        public HomeController(ILogger<HomeController> logger, ICoordinatesService coordinatesService)
        {
            _logger = logger;
            _coordinatesService = coordinatesService;
            _rovers = new List<ReactRoverResultModel>();
        }

        public IActionResult Index()
        {
            // Loads empty model
            var model = new CoordinatesModel();
            model.Results = new List<RoverResultModel>();
            model.Results.Add(new RoverResultModel());
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CoordinatesModel model)
        {
            // sets result for current rover
            var currentRover = model.Results[model.Results.Count - 1];

            try
            {
                // sets end coordinates
                _coordinatesService.SetEnd(model.MaxCoordinates);

                // sets start position of rover
                _coordinatesService.SetCurrentPosition(currentRover.CurrentPosition);

                // calculates coordinates depending on route
                currentRover.Result = _coordinatesService.Calculate(currentRover.Movement);

                // adds result to model
                model.Results.Add(new RoverResultModel());
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", ex.Message);
            }

            return View(model);
        }

        public IActionResult React()
        {
            return View();
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult AddRover(int? id)
        {
            return Json(new ReactRoverResultModel
            {
                Id = id ?? 1,
                MaxCoordinates = string.Empty,
                CurrentPosition = string.Empty,
                Movement = string.Empty,
                Result = string.Empty
            });
        }

        [HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Calculate(ReactRoverResultModel rover)
        {
            var currentRover = rover;
            // sets end coordinates
            _coordinatesService.SetEnd(rover.MaxCoordinates);

            // sets start position of rover
            _coordinatesService.SetCurrentPosition(currentRover.CurrentPosition);

            // calculates coordinates depending on route
            currentRover.Result = _coordinatesService.Calculate(currentRover.Movement);

            return Json(currentRover);
        }
    }
}
