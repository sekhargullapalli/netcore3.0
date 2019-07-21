using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.gRPCServer
{
    public static class ChinookExtensions
    {
        public static DateTime ConvertToDateTime (this byte[] bytes) =>
            DateTime.Parse(System.Text.Encoding.UTF8.GetString(bytes));
    }
}
