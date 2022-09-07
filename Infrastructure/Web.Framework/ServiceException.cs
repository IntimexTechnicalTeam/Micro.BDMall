namespace Web.Framework
{
    public class ServiceException : Exception
    {
        public ServiceException()
        {

        }
        public ServiceException(string msg) : base(msg)
        {

        }
    }

    /// <summary>
    /// Api请求的异常
    /// </summary>
    public class ApiServiceException : Exception
    {
        public ApiServiceException()
        {

        }
        public ApiServiceException(int errorCode, string msg) : base(msg)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }
    }

    /// <summary>
    /// 输入检查错误
    /// </summary>
    public class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }
        public InvalidInputException(string message)
            : base(message)
        {
        }
    }

    public class BLException : System.Exception
    {
        public BLException()
        {

        }

        public BLException(string message) : base(message)
        {

        }

        public BLException(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}