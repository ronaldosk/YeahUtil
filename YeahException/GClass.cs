using System;
namespace YeahException
{
    public class YeahExceptionDesc
    {
        public YeahExceptionDesc()
        {
        }

        public static string ExceptionDesc(YeahExceptionType exceptionType)
        {
            string outMsg = "";
            switch(exceptionType)
            {
                case YeahExceptionType.ConstructFailure:

                    outMsg = "对象构造时发生异常";
                    break;
                case YeahExceptionType.InvalidFilePath:
                    outMsg = "无效的文件路径名！";
                    break;
                case YeahExceptionType.FileNotExists:
                    outMsg = "文件不存在！";
                    break;
                case YeahExceptionType.ClassTypeNotExists:
                    outMsg = "不存在的对象类型！";
                    break;
                default :
                    outMsg = "";
                    break;         
            }

            return outMsg;
        }
    }

    public enum YeahExceptionType
    {
        /// <summary>
        /// 未知异常.
        /// </summary>
        UnknowError = 0,
        /// <summary>
        /// 对象构造时异常
        /// </summary>
        ConstructFailure = 1,
        /// <summary>
        /// 无效的文件路径名
        /// </summary>
        InvalidFilePath = 2,
        /// <summary>
        /// 文件不存在
        /// </summary>
        FileNotExists = 3,
        /// <summary>
        /// 不存在的对象类型
        /// </summary>
        ClassTypeNotExists = 4


    }
}
