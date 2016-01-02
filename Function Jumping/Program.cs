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

    static class FunctionJumperEventDriver_FOR_BOO
    {
        public delegate void CallAction();  //I need the same signature as the event ya want to override... 
                                            //Ex: 
                                                //If the method ya want to overide is named:
                                                    //int Killme(int howmanytimes, DieType dieType)
                                                //Then renaim this method too:
                                                    //public delegate int CallAction(int howmanytimes, DieType dieType)
                                            //Understand? PS: I don't care about its access level...

        public static event CallAction OnCalled;

        private static bool inited = false;

        //Need to check if OnCalled is null... sorryfor extra function :(
        public static void OnCalledGate()   //I need the same signature as the event ya want to override here too! 
                                            //Ex: 
                                                //If the method ya want to overide is named:
                                                    //int Killme(int howmanytimes, DieType dieType)
                                                //Then renaim this method too:
                                                    //public int OnCalledGate(int howmanytimes, DieType dieType)
                                            //Understand? PS: I don't care about its access level...
        {
            if (OnCalled != null)
                OnCalled(); //Pass everything forward... keep the paremetters the same as OnCalledGate()'s 
        }

        //TODO: HARDCODE VALUE
        public static MethodInfo functionToOverride {  //Set this at runtime... or hardcode it if ya figure out how...

            get { return functionToOverride; }

            set
            {
                if (value == null)
                {
                    Console.WriteLine("NO FUNCTION");
                    return;
                }

                if (inited)
                {
                    Console.WriteLine("Already inited... too late.");
                    return;
                }

                functionToOverride = value;
            }
        }

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            if (functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            MethodInfo eventMethod = Type.GetMethod(MethodBase.GetCurrentMethod().Name); 

            if (eventMethod == null)
            {
                Console.WriteLine("We failed to get out eventMethod...");
                return 404;
            }

            //Get where the function is...
            int Source_Base = functionToOverride.MethodHandle.GetFunctionPointer().ToInt32();
            int Destination_Base = eventMethod.MethodHandle.GetFunctionPointer().ToInt32();

            //Calculate the diffrence between the 2 function's locations
            int offset_raw = Destination_Base - Source_Base;

            uint* Pointer_Raw_Source = (uint*)Source_Base;

            // [WEIRD POINTER MATH] //
            //From RawCode
            *(Pointer_Raw_Source + 0) = 0xE9909090;
            *(Pointer_Raw_Source + 1) = (uint)(offset_raw - 8);
            // [/WEIRD POINTER MATH] //

            inited = true;

            return 0;
        }

        //AS RELABLE AS YOUTUBE!
        static void subToThisEvent(Delegate newSub)
        {
            OnCalled += (CallAction)newSub;
        }

        static void unsubToThisEvent(Delegate oldSub)
        {
            OnCalled -= (CallAction)oldSub;
        }
    }
}
