using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace AppCache
{
    public interface ICacheApp 
    {
        string GetValue(string Url);
        bool Add(string name, string Url);
        bool Lock(string name, string Url);
        void Update(string name, string Url);
        void Delete( string name ,string Url);
    }
    public class CacheApp: ICacheApp
    {
        public string GetValue(string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(Url) as string;
        }
        public bool Lock(string userName, string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (GetValue(Url) == null)
            {
                Add(userName, Url);
            }
            var Name = memoryCache.Get(Url) as string;
            if(Name == userName)
            {
                return false;
            }
            return true;
        }

        public bool Add(string name, string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(Url, name, DateTime.Now.AddMinutes(10));
        }

        public void Update(string name,string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(Url, name, DateTime.Now.AddMinutes(10));
        }

        public void Delete(string name, string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            var Name = memoryCache.Get(Url) as string;
            if (memoryCache.Contains(Url) && Name == name)
            {
                memoryCache.Remove(Url);
            }
        }
    }
}
