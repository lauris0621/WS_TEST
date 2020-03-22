using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WS_TEST_LIBRARY.Models
{
    public class CoordinatesModel
    {
        [Display(Name = "Max Coordinates")]
        public string MaxCoordinates { get; set; }

        public List<RoverResultModel> Results { get; set; }
    }
}
