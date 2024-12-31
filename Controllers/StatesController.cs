using LearnAPI.Model;
using LearnAPI.Repositories.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : ControllerBase
    {
        public readonly IStateRepository _stateRepository;
        public StatesController(IStateRepository stateRepository)
        {
            this._stateRepository = stateRepository;
        }
        //[HttpGet]
        //[Route("index")]
        //public List<StateModel> Index()
        //{
        //    var states = _stateRepository.List();
        //    return states;
        //}
        //[HttpGet]
        //[Route("index")]
        //public async Task<List<StateModel>> Index()
        //{
        //    var states =  _stateRepository.List() ?? new List<StateModel>();
        //    return states; // Accepting the nullable value
        //}

        [HttpGet]
        [Route("index")]
        public ActionResult<List<StateModel>> Index()
        {
            var states = _stateRepository.GetStatesDataAdapterAndStoredProcedure();

            if (states == null || !states.Any())
                return NotFound(new { Message = "No states found." }); // Return 404 if no data

            return Ok(states); // Return 200 with the list of states
        }
    }
}
