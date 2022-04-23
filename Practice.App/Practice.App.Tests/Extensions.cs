using System;

namespace Practicer.App.Tests
{
    internal static class Extensions
    {
        public static THandler CreateHandler<THandler>(params object[] repositories)
        { 
            return (THandler) Activator.CreateInstance(typeof(THandler), repositories);
        }
    }
}
