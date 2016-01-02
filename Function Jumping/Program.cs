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
            //Initial try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI();
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Init Events
            FunctionJumperEventDriver_For_A.Init();
            FunctionJumperEventDriver_For_B.Init();
            FunctionJumperEventDriver_For_C.Init();

            //Add functions
            FunctionJumperEventDriver_For_A.OnCalled += RimFunctions.A;
            FunctionJumperEventDriver_For_B.OnCalled += RimFunctions.B;
            FunctionJumperEventDriver_For_C.OnCalled += RimFunctions.C;

            //Secound try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI();
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Remove functions
            FunctionJumperEventDriver_For_A.OnCalled -= RimFunctions.A;
            FunctionJumperEventDriver_For_B.OnCalled -= RimFunctions.B;
            FunctionJumperEventDriver_For_C.OnCalled -= RimFunctions.C;

            //Third try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI();
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Multiple on each functions
            FunctionJumperEventDriver_For_A.OnCalled += RimFunctions.A;
            FunctionJumperEventDriver_For_B.OnCalled += RimFunctions.A;
            FunctionJumperEventDriver_For_A.OnCalled += RimFunctions.B;
            FunctionJumperEventDriver_For_B.OnCalled += RimFunctions.B;
            FunctionJumperEventDriver_For_C.OnCalled += RimFunctions.C;

            //Fourth try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI();
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!!");
            Console.ReadLine();
        }

        static private int C(int num)
        {
            Console.WriteLine("C OVERRIDE");
            return num * num;
        }
    }

    class RimFunctions
    {
        static public void A()
        {
            Console.WriteLine("A NORM");
        }
        static public void B()
        {
            Console.WriteLine("B NORM");
        }
        static public int C(int num)
        {
            Console.WriteLine("C NORM");
            return num;
        }
    }

    static class FunctionJumperEventDriver_For_A
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

        //Need to check if OnCalled is null... sorry for extra function :(
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

        private static string functionToOverrideName = "Function_Far_Far_Away.RimAPIFunctions.AAPI()";

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            //Drop the brackets...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - 2);

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (BindingFlags)(60) /*Inctance, Non-Public, Public and Static functions included in search*/);

            if (functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

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
    }

    static class FunctionJumperEventDriver_For_B
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

        //Need to check if OnCalled is null... sorry for extra function :(
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

        private static string functionToOverrideName = "Function_Far_Far_Away.RimAPIFunctions.BAPI()";

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            //Drop the brackets...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - 2);

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (BindingFlags)(60) /*Inctance, Non-Public, Public and Static functions included in search*/);

            if (functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

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
    }

    static class FunctionJumperEventDriver_For_C
    {
        public delegate int CallAction(int num);  //I need the same signature as the event ya want to override... 
                                                  //Ex: 
                                                  //If the method ya want to overide is named:
                                                  //int Killme(int howmanytimes, DieType dieType)
                                                  //Then renaim this method too:
                                                  //public delegate int CallAction(int howmanytimes, DieType dieType)
                                                  //Understand? PS: I don't care about its access level...

        public static event CallAction OnCalled;

        private static bool inited = false;

        //Need to check if OnCalled is null... sorry for extra function :(
        public static void OnCalledGate(int num)   //I need the same signature as the event ya want to override here too! 
                                            //Ex: 
                                            //If the method ya want to overide is named:
                                            //int Killme(int howmanytimes, DieType dieType)
                                            //Then renaim this method too:
                                            //public void OnCalledGate(int howmanytimes, DieType dieType)
                                            //Understand? PS: I don't care about its access level... OR its return type...
        {
            if (OnCalled != null)
                OnCalled(num); //Pass everything forward... keep the paremetters the same as OnCalledGate()'s 
        }

        private static string functionToOverrideName = "Function_Jumping.Program.C()";

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            //Drop the brackets...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - 2);

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (BindingFlags)(60) /*Inctance, Non-Public, Public and Static functions included in search*/);

            if (functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

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
    }
}

/* GENERIC EVENT */
/*     static class FunctionJumperEventDriver_For_A
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

        //Need to check if OnCalled is null... sorry for extra function :(
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

        private static string functionToOverrideName = "Function_Far_Far_Away.RimAPIFunctions.AAPI()";

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            //Drop the brackets...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - 2);

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = functionToOverrideName.Remove(functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (BindingFlags)(60)); //Inctance, Non-Public, Public and Static functions included in search

            if (functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

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
    }
 */
