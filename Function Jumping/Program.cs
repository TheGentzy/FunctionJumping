using System;
using System.Reflection;

namespace Function_Far_Far_Away
{
    class RimAPIFunctions
    {
        static internal void AAPI()
        {
            Console.WriteLine("A API");
        }
        static public void BAPI()
        {
            Console.WriteLine("B API");
        }
    }
}

namespace Function_Jumping
{
    class Program
    {
        static void Main(string[] args)
        {

            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI();
            C();

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Hook Method
            FunctionJumper.HookMethodFromTo("Function_Far_Far_Away.RimAPIFunctions.AAPI()",
                                            "Function_Jumping.RimFunctions.A()");

            FunctionJumper.HookMethodFromTo("Function_Far_Far_Away.RimAPIFunctions.BAPI()",
                                           "Function_Jumping.RimFunctions.B()");

            FunctionJumper.HookMethodFromTo("Function_Jumping.Program.C()",
                                            "Function_Jumping.RimFunctions.C()");

            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI();
            C();

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();
        }

        static private void C()
        {
            Console.WriteLine("C Override");
        }
    }

    class RimFunctions
    {
        static public void A()
        {
            Console.WriteLine("A NORM");
        }
        static protected void B()
        {
            Console.WriteLine("B NORM");
        }
        static private void C()
        {
            Console.WriteLine("C NORM");
        }
    }

    class FunctionJumper
    {
        static public int HookMethodFromTo (string source_method_name, string destination_method_name)
        {
            Type source_type;
            Type destination_type;

            //Drop the brackets...
            source_method_name      = source_method_name.Remove     (source_method_name.Length      - 2);
            destination_method_name = destination_method_name.Remove(destination_method_name.Length - 2);

            //Split MethodName into its parts...
            string[] source_method_name_parts      = source_method_name     .Split('.');
            string[] destination_method_name_parts = destination_method_name.Split('.');

            //Get the name of the function
            string source_method_name_end      = source_method_name_parts     [source_method_name_parts.Length - 1];
            string destination_method_name_end = destination_method_name_parts[source_method_name_parts.Length - 1];

            //Console.WriteLine(source_method_name_end + " AND " + destination_method_name_end);

            //Get the class name...
            source_method_name      = source_method_name     .Remove(source_method_name     .Length - source_method_name_end     .Length - 1);
            destination_method_name = destination_method_name.Remove(destination_method_name.Length - destination_method_name_end.Length - 1);

            //Console.WriteLine(source_method_name + " AND " + destination_method_name);

            //Convert the string into a class type
            source_type      = Type.GetType(source_method_name);
            destination_type = Type.GetType(destination_method_name);

            //Check for errors...
            if (source_type == null || destination_type == null)
            {
                Console.WriteLine("NO Type - 404 ERROR");

                return 404;
            }

            //Grab the methodinfo...
            MethodInfo Source_Method      = source_type     .GetMethod(source_method_name_end,      (BindingFlags)(60));
            MethodInfo Destination_Method = destination_type.GetMethod(destination_method_name_end, (BindingFlags)(60));

            //Call REAL method
            return HookMethodFromTo(Source_Method, Destination_Method);
        }

        unsafe static public int HookMethodFromTo(MethodInfo Source_Method, MethodInfo Destination_Method)
        {
            if (Source_Method == null || Destination_Method == null)
            {
                Console.WriteLine("NO MethodInfo - 404 ERROR");

                return 404;
            }

            //Get where the function is...
            int Source_Base = Source_Method.MethodHandle.GetFunctionPointer().ToInt32();
            int Destination_Base = Destination_Method.MethodHandle.GetFunctionPointer().ToInt32();

            //Calculate the diffrence between the 2 function's locations
            int offset_raw = Destination_Base - Source_Base;

            uint* Pointer_Raw_Source = (uint*)Source_Base;

            // [WEIRD POINTER MATH] //
            //From RawCode
            *(Pointer_Raw_Source + 0) = 0xE9909090;
            *(Pointer_Raw_Source + 1) = (uint)(offset_raw - 8);
            // [/WEIRD POINTER MATH] //


            return 0;
        }
    }
}
