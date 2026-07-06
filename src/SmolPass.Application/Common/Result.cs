using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmolPass.Application.Common
{
    public sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error {  get; }

        private readonly T? _value;

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            _value = value;
            Error = error;
        }

        //on ne lit que les succès. Un echec est traité comme un bug d'usage -> on part en exception
        public T Value => IsSuccess? _value! : throw new InvalidOperationException("Lecture de valeur interdite sur un Result");

        // Les deux seules portes d'entrée
        public static Result<T> Success(T value) => new Result<T>(true, value, null);
        public static Result<T> Failure(string error) => new Result<T>(false, default, error);
    }
}
