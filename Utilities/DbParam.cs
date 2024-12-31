using System.Data;

namespace LearnAPI.Utilities
{
    public struct DbParam
    {
        public string Param;

        public object Value;

        public SqlDbType? DbType;
    }
}
