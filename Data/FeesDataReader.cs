using Newtonsoft.Json;
using System.IO;
using KofileDotNet.Models;

namespace KofileDotNet.Data
{
    public class FeesDataReader
    {
        private static FeesDataReader instance;

         public FeeType[] FeeList { get; private set;}

        private FeesDataReader()
        {
            this.FeeList = Newtonsoft.Json.JsonConvert.DeserializeObject<FeeType[]>(File.ReadAllText("json/fees.json"));
        }

        public static FeesDataReader Instance
        {
            get
            {
                if(instance == null)
                    instance = new FeesDataReader();

                return instance;
            }
        }
    }
}