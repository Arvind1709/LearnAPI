using LearnAPI.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LearnAPI.VModel
{
    public class UserVM
    {
        public UserModel? File { get; set; }
        public List<UserModel>? Files { get; set; }

        [Display(Name = "प्रथम नाम", Prompt = "कर्मचारी का प्रथम नाम टाइप करें")]
        public string? FirstNameFilter { get; set; }

        [Display(Name = "अंतिम नाम", Prompt = "कर्मचारी का अंतिम नाम टाइप करें")]
        public string? lastNameFilter { get; set; }

        // dropdowns --
        public List<SelectListItem>? DesignationDropDown { get; set; }
        public List<SelectListItem>? StateDropDown { get; set; }
        public List<SelectListItem>? DistrictDropDown { get; set; }
        public List<SelectListItem>? GenderDropDown { get; set; }
        public List<SelectListItem>? CasteDropDown { get; set; }
    }
}
