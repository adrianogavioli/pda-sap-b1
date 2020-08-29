using Newtonsoft.Json;

namespace Frattina.CrossCutting.JSonTools
{
    public static class Utils
    {
        public static T DeserializarObj<T>(string dataJson)
        {
            return JsonConvert.DeserializeObject<T>(dataJson);
        }

        public static string SerializarObj<T>(T dataObj)
        {
            return JsonConvert.SerializeObject(dataObj);
        }
    }
}
