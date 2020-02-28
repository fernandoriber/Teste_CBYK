using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace TesteCapta.Helpers
{
    public class Utils
    {
        public static bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            return (current == NetworkAccess.Internet ? true : false);
        }
    }
}
