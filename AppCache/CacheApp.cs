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
        string GetValue(string Key);
        T GetValue<T>(string Key);
        void SetValue<T>(string Key, T value);
        bool Add(string name, string Url);
        bool AddInfinity(string Key, string value);
        bool AddLock30Minute(string name, string Method);
        bool Lock(string name, string Url);
        bool LockUpload(string userName, string Url, bool infinity = false);
        void Update(string name, string Url);
        void Delete( string name ,string Url);
        void Delete(string Key);
        bool isLock(string Key);
        void UpdateProgres(string Key, string value);

    }
    public class CacheApp: ICacheApp
    {
        /// <summary>
        /// Установить прогресс очистить автоматически через 10 минут
        /// </summary>
        /// <param name="name">Название ключа</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
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
        /// <summary>
        /// Получение информации о прогрессе по ключу
        /// </summary>
        /// <param name="Name">Название ключа</param>
        /// <returns></returns>
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
        /// <summary>
        /// Получить значение из кеша типа string
        /// </summary>
        /// <param name="Key">Название ключа</param>
        /// <returns></returns>
        public string GetValue(string Key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(Key) as string;
        }
        /// <summary>
        /// Generic метод получение данных из кеша
        /// </summary>
        /// <typeparam name="T">Обьект</typeparam>
        /// <param name="Key">Название ключа</param>
        /// <returns></returns>
        public T GetValue<T>(string Key)
        {
            try
            {
                MemoryCache memoryCache = MemoryCache.Default;
                return (T)memoryCache.Get(Key);
            }catch(Exception e)
            {
                return default;
            }
        }
        /// <summary>
        /// Generic установки данных в кеше
        /// </summary>
        /// <typeparam name="T">Обьект</typeparam>
        /// <param name="Key">Название ключа</param>
        /// <param name="value">Значение</param>
        public void SetValue<T>(string Key, T value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(Key, value, DateTime.Now.AddHours(10));
        }
        /// <summary>
        ///  Проверка блокировки страницы или обьекта c последующей блокировкой
        /// </summary>
        /// <param name="Key">Название ключа</param>
        /// <returns></returns>
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
        /// <summary>
        /// Заблокировать страницу или обьект
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="Url">Url страницы или обьекта</param>
        /// <returns></returns>
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
        /// <summary>
        /// Заблокировать загрузку
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="Url">Url страницы или обьекта</param>
        /// <returns></returns>
        public bool LockUpload(string userName, string Url, bool infinity = false)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (GetValue(Url) == null)
            {
                if(infinity)
                    AddInfinity(userName, Url);
                else
                    Add(Url, userName);
            }
            var Name = memoryCache.Get(Url) as string;
            if (Name == userName)
            {
                return false;
            }
            return true;
        }
        public bool AddLock30Minute(string name, string Method)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(name, Method, DateTime.Now.AddMinutes(30));
        }
        public bool Add(string value, string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, DateTime.Now.AddMinutes(10));
        }
        public bool AddInfinity(string Key, string value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(Key, value, DateTime.Now.AddDays(1));
        }

        public void Update(string key,string value)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Set(key, value, DateTime.Now.AddMinutes(10));
        }

        public void Delete(string value, string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            var Name = memoryCache.Get(key) as string;
            if (memoryCache.Contains(key) && Name == value)
            {
                memoryCache.Remove(key);
            }
        }
        public void Delete(string Key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            memoryCache.Remove(Key);
        }
    }
}
