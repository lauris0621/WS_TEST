using System.ComponentModel.DataAnnotations;

namespace WS_TEST_LIBRARY.Models
{

    public class RoverResultModel
    {
        public int Id { get; set; }

        [Display(Name = "Current Position")]
        public string CurrentPosition { get; set; }

        [Display(Name = "Movement")]
        public string Movement { get; set; }

        [Display(Name = "Result")]
        public string Result { get; set; }
    }
}
