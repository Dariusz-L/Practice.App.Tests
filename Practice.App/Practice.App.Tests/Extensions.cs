using System;

namespace Practicer.App.Tests
{
    internal static class Extensions
    {
        public static THandler CreateHandler<THandler>(object repository)
        { 
            return (THandler) Activator.CreateInstance(typeof(THandler), new object[] { repository });
        }
    }
}
