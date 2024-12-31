using LearnAPI.Model;
using LearnAPI.Utilities;

namespace LearnAPI.Repositories.IRepository
{
    public interface IStateRepository
    {
        ResultInfo Delete(int stateId);
        StateModel? Get(int stateId);
        ResultInfo Insert(StateModel model);
        List<StateModel> List(int? stateId = null);
        List<StateModel> GetStatesDataAdapterAndStoredProcedure(int? stateId = null);
        ResultInfo Update(StateModel model);
    }
}
