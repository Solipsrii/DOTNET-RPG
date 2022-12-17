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
        public T? data { get; set; } //the generic, the main component. Carries all of the information of the original format.

        //additional information stored here:

        ///<summary>
        ///<value>sucess</value> reports whether the given operation of the Service was successful or not.
        ///</summary> 
        public bool success { get; set; } = true;

        ///<summary>
        ///<value>message</value> carries notably error messages to be displayed in a try{}catch{} statements.
        ///</summary> 
        public string message { get; set; } = "";
        ///<summary>

        public void setErrorMessage(string msg){
            message = msg;
            success = false;
        }
    }
}