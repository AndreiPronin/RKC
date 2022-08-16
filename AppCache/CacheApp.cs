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
        bool AddProgress(string name, string value);
        void UpdateProgress(string name, string value);
        string GetValueProgress(string Name);
        string GetValue(string Url);
        bool AddLock(string name, string Method);
        bool Add(string name, string Url);
        bool Lock(string name, string Url);
        void Update(string name, string Url);
        void Delete( string name ,string Url);
        bool isLock(string Key);

    }
    public class CacheApp: ICacheApp
    {
        public bool AddProgress(string name, string value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(name, value, DateTime.Now.AddMinutes(200));
        }
        public string GetValueProgress(string Name)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(Name) as string;
        }
        public void UpdateProgress(string name, string value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(name, value, DateTime.Now.AddMinutes(10));
        }
        public string GetValue(string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(Url) as string;
        }
        public bool isLock(string Key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (GetValue(Key) == null)
            {
                Add(Key, "Lock");
                return false;
            }
            var Name = memoryCache.Get(Key) as string;
            if (Name == "Lock")
            {
                return true;
            }
            return false;
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
        public bool AddLock(string name, string Method)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(name, Method, DateTime.Now.AddMinutes(10));
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
