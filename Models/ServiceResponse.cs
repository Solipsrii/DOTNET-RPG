using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Models
{
    //an OBJECT WRAPPER, contains a generic object (data), which is identital as if using the object itself (i.e: character.name, wrapper.data.name)
    //the added benefit here is front-end support. More information to directly draw from this specific object!
    public class ServiceResponse<T>
    {
        public T? data {get;set;} //the generic, the main component. Carries all of the information of the original format.
        
        //additional information stored here:
        public bool success { get; set; } = true;
        public string message { get; set; } = "";
    }
}