using System.Collections.Generic;
using System.Data;

namespace LearnAPI.Utilities
{
    public class ResultInfo
    {
        public string Title { get; set; } = "Success";


        public string Message { get; set; } = "";


        public string Result { get; set; } = "";


        public string AdditionalInfo { get; set; } = "";


        public List<string> AdditionalItems { get; set; }

        public DataTable Table { get; set; }

        public object Object { get; set; }

        public dynamic Data { get; set; }

        public bool HasError { get; set; } = false;


        public int ErrorNo { get; set; } = 0;


        public bool IsDBError { get; set; } = false;


        public string SystemMessage { get; set; } = "";
        public DbExceptions ErrorType { get; set; } = DbExceptions.None;


        public string ErrorMessage
        {
            set
            {
                Message = value;
                HasError = true;
                Title = "Error";
                ErrorNo = 0;
            }
        }

        public ResultInfo SetSuccess(string message)
        {
            HasError = false;
            Title = "Success";
            SystemMessage = Message;
            Message = message;
            ErrorNo = 0;
            return this;
        }

        public ResultInfo SetError(string message, int errorNo = 0, bool handyError = false)
        {
            HasError = true;
            Title = "Error";
            SystemMessage = message;
            Message = (handyError ? HandyErrorMsg(errorNo, message) : message);
            ErrorNo = errorNo;
            return this;
        }

        public string GetSystemMessage()
        {
            return SystemMessage;
        }

        private string HandyErrorMsg(int errorNo = 0, string systemMessage = "")
        {
            if (errorNo != 0)
            {
                IsDBError = true;
            }

            if (errorNo == 2601 || errorNo == 2627)
            {
                ErrorType = DbExceptions.DuplicateRecordException;
                return "You are entering some duplicate entry.";
            }

            switch (errorNo)
            {
                case 547:
                    ErrorType = DbExceptions.ForeignKeyException;
                    return "Record is referred in some other file(s). You can't delete the record.";
                case 8152:
                    ErrorType = DbExceptions.FiledLenghtExceedingException;
                    return "Some entries lengths are exceeding than the defined size Please review your entries and try again.";
                case 8114:
                    ErrorType = DbExceptions.TypeMismatchedException;
                    return "Provided data is not compatible as per our database. Please review your entries and try again.";
                case 15530:
                    ErrorType = DbExceptions.DBConnectionException;
                    return "We are not able to connect the database this time. Please try again later.";
                case 1205:
                    ErrorType = DbExceptions.DeadlockException;
                    return "Deadlock position occurred. Please try again later.";
                default:
                    ErrorType = DbExceptions.UnknownException;
                    if (systemMessage.ToUpper().StartsWith("[DBMSG]"))
                    {
                        return systemMessage.TrimStart();

                       // return systemMessage.TrimStart("[DBMSG]");
                    }

                    return "An error occurred during the execution.";
            }
        }
    }
}
