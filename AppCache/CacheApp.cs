using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppCache
{
    public interface ICacheApp 
    {
        bool AddProgress(string name, string value);
        void UpdateProgress(string name, string value);
        string GetValueProgress(string Name);
        string GetValue(string Url);
        T GetValue<T>(string Key);
        void SetValue<T>(string Key, T value);
        bool AddLock(string name, string Method);
        bool Add(string name, string Url);
        bool AddLock30Minute(string name, string Method);
        bool Lock(string name, string Url);
        void Update(string name, string Url);
        void Delete( string name ,string Url);
        void Delete(string Key);
        bool isLock(string Key);
        void UpdateProgres(string Key, string value);

    }
    public class CacheApp: ICacheApp
    {
        public bool AddProgress(string name, string value)
        {
            if (GetValue(name) == null)
            {
                MemoryCache memoryCache = MemoryCache.Default;
                return memoryCache.Add(name, value, DateTime.Now.AddMinutes(10));
            }
            else
            {
                UpdateProgres(name, value);
                return false;
            }
            
        }
        public string GetValueProgress(string Name)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(Name) as string;
        }
        /// <summary>
        /// Обновление прогресс цифры
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void UpdateProgress(string name, string value)
        {
            if (Convert.ToDouble(value) > Convert.ToDouble(GetValueProgress(name)))
            {
                MemoryCache memoryCache = MemoryCache.Default;
                memoryCache.Set(name, value, DateTime.Now.AddMinutes(10));
            }
        }
        /// <summary>
        /// Обновление прогресс текстовое значение
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        public void UpdateProgres(string Key, string value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(Key, value, DateTime.Now.AddMinutes(10));
        }
        public string GetValue(string Key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(Key) as string;
        }
        public T GetValue<T>(string Key)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                return (T)memoryCache.Get(Key);
            }catch(Exception e)
            {
                return default(T);
            }
        }
        public void SetValue<T>(string Key, T value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(Key, value, DateTime.Now.AddHours(10));
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
        public bool AddLock30Minute(string name, string Method)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(name, Method, DateTime.Now.AddMinutes(10));
        }
        public bool Add(string name, string Url)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(Url, name, DateTime.Now.AddMinutes(10));
        }

        public void Update(string key,string value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(key, value, DateTime.Now.AddMinutes(10));
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
        public void Delete(string Key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Remove(Key);
        }
    }
}
