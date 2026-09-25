using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LumineersAccessories.Infrastructure
{
    /// <summary>
    /// Small JSON helpers for storing typed objects in session.
    /// </summary>
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
            => session.SetString(key, JsonConvert.SerializeObject(value));

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
