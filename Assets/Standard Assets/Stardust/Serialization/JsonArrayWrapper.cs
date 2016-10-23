namespace Stardust
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    [Serializable]
    public class JsonArrayWrapper<T>
    {
        public T[] array;
    }

    [Serializable]
    public class JsonListWrapper<T>
    {
        public List<T> list;
    }
}