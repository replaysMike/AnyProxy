using System;
using System.Collections.Generic;
using System.Text;

namespace AnyProxy
{
    public class Proxy<T>
    {
        public Type UnderlyingType { get; }
        public T Object { get; }

        public Proxy()
        {

        }

        public Proxy(T obj)
        {
            UnderlyingType = typeof(T);
            Object = obj;
        }

        public void AddInterceptor()
        {

        }
    }
}
