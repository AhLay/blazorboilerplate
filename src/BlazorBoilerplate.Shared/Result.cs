using System;
using System.Threading.Tasks;

namespace BlazorBoilerplate.Shared
{
    public static class ResultExtensions
    {
        public static Task<Result> AsTask(this Result result)
        {
            return
                Task.FromResult(result);
        }

        public static Task<Result<T>> AsTask<T>(this Result<T> result)
        {
            return
                Task.FromResult(result);
        }

    }

    public class Result
    {
        protected Result(string message,bool isError)
        {
            Message = message;
            Failed = isError;
        }

        public string Message { get; }
        public bool Failed { get; }

        public static Result<T> Error<T>(T value, Exception exception)
        {
            return
                new Result<T>(exception.Message, value,true);
        }

        public static Result Error(Exception exception)
        {
            return
                new Result(exception.Message,true);
        }

        public static Result Error(string message)
        {
            return
                new Result(message,true);
        }

        public static Result<T> Success<T>(T value, string message)
        {
            return
                new Result<T>(message, value,false);
        }

        
        public static Result Success(string message)
        {
            return
                new Result(message,false);
        }

    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        public Result(string message, T value,bool isError) : base(message,isError)
        {
            Value = value;
        }
    }

}
