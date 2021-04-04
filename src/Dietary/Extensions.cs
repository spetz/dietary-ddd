using System;
using System.Threading.Tasks;

namespace Dietary
{
    public static class Extensions
    {
        public static async Task<T> OrElseThrow<T>(this Task<T> task, Func<Exception> exception)
        {
            var result = await task;
            if (result is null)
            {
                throw exception();
            }

            return result;
        }
    }
}