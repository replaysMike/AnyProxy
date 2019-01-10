namespace AnyProxy.Extensions
{
    public static class ProxyExtensions
    {
        /// <summary>
        /// Create a proxied object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Proxy<T> Proxy<T>(this T obj)
        {
            var provider = new ProxyProvider();
            return provider.CreateProxy<T>(obj);
        }
    }
}
