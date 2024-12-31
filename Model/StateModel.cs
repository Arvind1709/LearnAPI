using System.ComponentModel.DataAnnotations;

namespace LearnAPI.Model
{
    public class StateModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required!")]
        [Display(Name = "राज्य का नाम", Prompt = "")]
        public string Name { get; set; }
    }
}
