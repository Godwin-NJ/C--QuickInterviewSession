using static System.Net.WebRequestMethods;
using System;
using System.Collections.Generic;
//using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace understandingJWT
{
    public class CleanJson
    {
        public  string Cleanupjson()
        {
            string urlJson = "https://coderbyte.com/api/challenges/json/json-cleaning";

            var getData = new HttpClient().GetStringAsync(urlJson).Result;
            Console.WriteLine(getData);            
          
            var formatJsonData = FormatData(getData);
          
            return formatJsonData;
        }



        public static string FormatData(string dataJson) 
        {
            //var data = JsonSerializer.Deserialize<dynamic>(dataJson);
            dynamic data = JsonConvert.DeserializeObject<Dictionary<string,dynamic>>(dataJson);
            Dictionary<string,dynamic> userDataMain = new Dictionary<string,dynamic>();

            foreach (var item in data)
            {
                var xItem = item.Key;
                var xvalue = item.Value;
                // Dictionary<string, dynamic> userDataMainNew = new Dictionary<string, dynamic>();
                List<dynamic> userDataMainNew = new List<dynamic> { };

                //focusing on the value of xValue
                if (xvalue is Int32 || xvalue is long)
                {
                  userDataMain.Add(xItem, xvalue);
                }
                else
                {
                    if (xvalue is String)
                    {
                        if (xvalue != "" && xvalue != "N\\/A" && xvalue != "-")
                        {
                          return userDataMain.Add(xItem, xvalue);
                        }
                      
                    }
                    else if (xvalue is object)
                    {
                        if (xvalue is Array) 
                        { 
                            List<string> xValueArray = new List<string> { };
                            foreach (var x in xvalue)
                            {
                                if (x != "" && x != "N\\/A" && x != "-")
                                {
                                    xValueArray.Add(x);
                               
                                }
                            }
                            return userDataMain.Add(xItem, xValueArray);
                        }

                        else if (xvalue.Count > 1)
                        {
                            foreach(var xitem in xvalue)
                            {
                                //var xItemnew = xitem.Key;
                                var xvaluenew = xitem.Value;
                                Console.WriteLine(xitem.Value);

                                if (xvaluenew != "" && xvaluenew != $"{"N/A"}" && xvaluenew != "-")
                                {
                                    userDataMainNew.Add(xvaluenew);
                                }                          
                               
                            }
                             userDataMain.Add(xItem, userDataMainNew);
                        }

                        else
                        {                        
                            Dictionary<string,dynamic> newobject = new Dictionary<string,dynamic>();
                            foreach(var x in xvalue)
                            {
                                if (x.Value != "" && x.Value != $"{"N/A"}" && x.Value != "-")
                                {
                                    newobject.Add(x.Name, x.Value);
                                }
                            }

                            userDataMain.Add(xItem, newobject);

                        }
                    
                    }
                   
                }
            }

            return JsonConvert.SerializeObject(userDataMain);
           // return JsonSerializer.Serialize(userDataMain);
        }
    }
}
