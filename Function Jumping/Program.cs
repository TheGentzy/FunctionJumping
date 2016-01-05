using System;
using System.Collections.Generic;
using System.Reflection;

namespace Function_Far_Far_Away
{
    class RimAPIFunctions
    {
        static internal void AAPI()
        {
            Console.WriteLine("A API");
        }
        static public unsafe void BAPI(int[] num, ref int*[] num2)
        {
            Console.WriteLine("B API");
        }
    }
}

namespace Function_Jumping
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            int[] num = new int[5]{ 5, 5, 5, 5, 5 };
            int*[] num2 = new int*[1] { (int*) 0 };

            //Initial try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Pass function ifo to events              //Its static, its internal,  no parameters, its location
            FunctionJumperEventDriver_For_A.FunctionInfo(     false,        false, new Type[] { }, "Function_Far_Far_Away.RimAPIFunctions.AAPI");
                                                       //Its static, its public,                                                                                              2 parameters, its location
            FunctionJumperEventDriver_For_B.FunctionInfo(     false,       true, new Type[] { typeof(int).MakeArrayType(), typeof(int).MakePointerType().MakeArrayType().MakeByRefType() }, "Function_Far_Far_Away.RimAPIFunctions.BAPI");
                                                       //Its static, its public,               1 parrameter, its location
            FunctionJumperEventDriver_For_C.FunctionInfo(     false,      false, new Type[] { typeof(int) }, "Function_Jumping.Program.C");

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
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Remove functions
            FunctionJumperEventDriver_For_A.OnCalled -= RimFunctions.A;
            FunctionJumperEventDriver_For_B.OnCalled -= RimFunctions.B;
            FunctionJumperEventDriver_For_C.OnCalled -= RimFunctions.C;

            //Third try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            //Multiple on each functions
            FunctionJumperEventDriver_For_C.OnCalled += RimFunctions.C;
            FunctionJumperEventDriver_For_C.OnCalled += D;

            //Fourth try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9));

            Console.WriteLine("HIT ENTER!!!");
            Console.ReadLine();
        }

        static private int C(int num)
        {
            Console.WriteLine("C OVERRIDE");
            return num * num;
        }

        static public int C(int num, int num2)
        {
            Console.WriteLine("C NORM" + num2);
            return num;
        }

        static private int D(int num)
        {
            Console.WriteLine("D OVERRIDE");
            return num * num * num;
        }
    }

    class RimFunctions
    {
        static public void A()
        {
            Console.WriteLine("A NORM");
        }
        static public unsafe void B(int[] num, ref int*[] num2)
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
        public delegate void CallAction();      //I need the same signature as the event ya want to override... 
                                                //Ex: 
                                                //If the method ya want to overide is named:
                                                //int Killme(int howmanytimes, DieType dieType)
                                                //Then renaim this method too:
                                                //public delegate int CallAction(int howmanytimes, DieType dieType)
                                                //Understand? PS: I don't care about its access level...

        public static event CallAction OnCalled;

        private static bool inited = false;
        public static bool allowedToInit { get; private set; } = false;

        //Need to check if OnCalled is null... sorry for extra function :(
        public static void OnCalledGate()       //I need the same signature as the event ya want to override here too! 
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

        private static string functionToOverrideName = ""; //Don't put parameters
        private static bool isInstanceOrVirtual = false;
        private static bool isPublic = false;

        static public int FunctionInfo(bool _isInstanceOrVirtual, bool _isPublic,
                                        Type[] parametersForFunction, string _functionToOverrideName)
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }
            allowedToInit = false;

            //Set varibals
            isInstanceOrVirtual = _isInstanceOrVirtual;
            isPublic = _isPublic;
            functionToOverrideName = _functionToOverrideName;

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = _functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = _functionToOverrideName.Remove(_functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo _functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic)
                                                                                                                     | (isInstanceOrVirtual ? BindingFlags.Instance : BindingFlags.Static)
                                                                                                                     | BindingFlags.FlattenHierarchy,
                                                                                                                     null, CallingConventions.Any, parametersForFunction, null);

            if (_functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            //Get OnCalledGate()
            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

            if (eventMethod == null)
            {
                Console.WriteLine("We failed to get the eventMethod...");
                return 404;
            }

            //Get where the function is...
            int _Source_Base = _functionToOverride.MethodHandle.GetFunctionPointer().ToInt32();
            int _Destination_Base = eventMethod.MethodHandle.GetFunctionPointer().ToInt32();

            //Pass forward method locations!
            Source_Base = _Source_Base;
            Destination_Base = _Destination_Base;

            //We now got the data we need
            allowedToInit = true;
            return 0;
        }

        private static int Source_Base, Destination_Base;

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            if (allowedToInit == false)
            {
                Console.WriteLine("I say your giving me invalid/no info");
                return -1;
            }

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
        public unsafe delegate void CallAction(int[] num, ref int*[] num2);      //I need the same signature as the event ya want to override... 
                                                //Ex: 
                                                //If the method ya want to overide is named:
                                                //int Killme(int howmanytimes, DieType dieType)
                                                //Then renaim this method too:
                                                //public delegate int CallAction(int howmanytimes, DieType dieType)
                                                //Understand? PS: I don't care about its access level...

        public static event CallAction OnCalled;

        private static bool inited = false;
        public static bool allowedToInit { get; private set; } = false;

        //Need to check if OnCalled is null... sorry for extra function :(
        public static unsafe void OnCalledGate(int[] num, ref int*[] num2)       //I need the same signature as the event ya want to override here too! 
                                                //Ex: 
                                                //If the method ya want to overide is named:
                                                //int Killme(int howmanytimes, DieType dieType)
                                                //Then renaim this method too:
                                                //public int OnCalledGate(int howmanytimes, DieType dieType)
                                                //Understand? PS: I don't care about its access level...
        {
            if (OnCalled != null)
                OnCalled(num, ref num2); //Pass everything forward... keep the paremetters the same as OnCalledGate()'s 
        }

        private static string functionToOverrideName = ""; //Don't put parameters
        private static bool isInstanceOrVirtual = false;
        private static bool isPublic = false;

        static public int FunctionInfo(bool _isInstanceOrVirtual, bool _isPublic,
                                        Type[] parametersForFunction, string _functionToOverrideName)
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }
            allowedToInit = false;

            //Set varibals
            isInstanceOrVirtual = _isInstanceOrVirtual;
            isPublic = _isPublic;
            functionToOverrideName = _functionToOverrideName;

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = _functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = _functionToOverrideName.Remove(_functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo _functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic)
                                                                                                                     | (isInstanceOrVirtual ? BindingFlags.Instance : BindingFlags.Static)
                                                                                                                     | BindingFlags.FlattenHierarchy,
                                                                                                                     null, CallingConventions.Any, parametersForFunction, null);

            if (_functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            //Get OnCalledGate()
            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

            if (eventMethod == null)
            {
                Console.WriteLine("We failed to get the eventMethod...");
                return 404;
            }

            //Get where the function is...
            int _Source_Base = _functionToOverride.MethodHandle.GetFunctionPointer().ToInt32();
            int _Destination_Base = eventMethod.MethodHandle.GetFunctionPointer().ToInt32();

            //Pass forward method locations!
            Source_Base = _Source_Base;
            Destination_Base = _Destination_Base;

            //We now got the data we need
            allowedToInit = true;
            return 0;
        }

        private static int Source_Base, Destination_Base;

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            if (allowedToInit == false)
            {
                Console.WriteLine("I say your giving me invalid/no info");
                return -1;
            }

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
        public delegate int CallAction(int num);    //I need the same signature as the event ya want to override... 
                                                    //Ex: 
                                                    //If the method ya want to overide is named:
                                                    //int Killme(int howmanytimes, DieType dieType)
                                                    //Then renaim this method too:
                                                    //public delegate int CallAction(int howmanytimes, DieType dieType)
                                                    //Understand? PS: I don't care about its access level...

        public static event CallAction OnCalled;

        private static bool inited = false;
        public static bool allowedToInit { get; private set; } = false;

        //Need to check if OnCalled is null... sorry for extra function :(
        public static int OnCalledGate(int num)       //I need the same signature as the event ya want to override here too! 
                                                //Ex: 
                                                //If the method ya want to overide is named:
                                                //int Killme(int howmanytimes, DieType dieType)
                                                //Then renaim this method too:
                                                //public int OnCalledGate(int howmanytimes, DieType dieType)
                                                //Understand? PS: I don't care about its access level...
        {
            if (OnCalled != null)
                return OnCalled(num); //Pass everything forward... keep the paremetters the same as OnCalledGate()'s
            return -1; 
        }

        private static string functionToOverrideName = ""; //Don't put parameters
        private static bool isInstanceOrVirtual = false;
        private static bool isPublic = false;

        static public int FunctionInfo(bool _isInstanceOrVirtual, bool _isPublic,
                                        Type[] parametersForFunction, string _functionToOverrideName)
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }
            allowedToInit = false;

            //Set varibals
            isInstanceOrVirtual = _isInstanceOrVirtual;
            isPublic = _isPublic;
            functionToOverrideName = _functionToOverrideName;

            //Split MethodName into its parts...
            string[] functionToOverrideNameParts = _functionToOverrideName.Split('.');

            //Get the name of the function
            string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

            //Get the class name...
            functionToOverrideName = _functionToOverrideName.Remove(_functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

            //Convert the string into a class type & Grab the methodinfo...
            MethodInfo _functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic)
                                                                                                                     | (isInstanceOrVirtual ? BindingFlags.Instance : BindingFlags.Static)
                                                                                                                     | BindingFlags.FlattenHierarchy,
                                                                                                                     null, CallingConventions.Any, parametersForFunction, null);

            if (_functionToOverride == null)
            {
                Console.WriteLine("NO functionToOverride - 404 ERROR...");
                return 404;
            }

            //Get OnCalledGate()
            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);
            MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

            if (eventMethod == null)
            {
                Console.WriteLine("We failed to get the eventMethod...");
                return 404;
            }

            //Get where the function is...
            int _Source_Base = _functionToOverride.MethodHandle.GetFunctionPointer().ToInt32();
            int _Destination_Base = eventMethod.MethodHandle.GetFunctionPointer().ToInt32();

            //Pass forward method locations!
            Source_Base = _Source_Base;
            Destination_Base = _Destination_Base;

            //We now got the data we need
            allowedToInit = true;
            return 0;
        }

        private static int Source_Base, Destination_Base;

        unsafe static public int Init()
        {
            if (inited == true)
            {
                Console.WriteLine("Already activated...");
                return 0;
            }

            if (allowedToInit == false)
            {
                Console.WriteLine("I say your giving me invalid/no info");
                return -1;
            }

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
/*
static class FunctionJumperEventDriver
{
    public delegate void CallAction();      //I need the same signature as the event ya want to override... 
                                            //Ex: 
                                            //If the method ya want to overide is named:
                                            //int Killme(int howmanytimes, DieType dieType)
                                            //Then renaim this method too:
                                            //public delegate int CallAction(int howmanytimes, DieType dieType)
                                            //Understand? PS: I don't care about its access level...

    public static event CallAction OnCalled;

    private static bool inited = false;
    public static bool allowedToInit { get; private set; } = false;
                                        
    //Need to check if OnCalled is null... sorry for extra function :(
    public static void OnCalledGate()       //I need the same signature as the event ya want to override here too! 
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

    private static string functionToOverrideName = ""; //Don't put parameters
    private static bool isInstanceOrVirtual = false;
    private static bool isPublic = false;

    static public int FunctionInfo(bool _isInstanceOrVirtual, bool _isPublic, 
                                    Type[] parametersForFunction,     string _functionToOverrideName)
    {
        if (inited == true)
        {
            Console.WriteLine("Already activated...");
            return 0;
        }
        allowedToInit = false;

        //Set varibals
        isInstanceOrVirtual    = _isInstanceOrVirtual;
        isPublic               = _isPublic;
        functionToOverrideName = _functionToOverrideName;

        //Split MethodName into its parts...
        string[] functionToOverrideNameParts = _functionToOverrideName.Split('.');

        //Get the name of the function
        string functionToOverrideNameEnd = functionToOverrideNameParts[functionToOverrideNameParts.Length - 1];

        //Get the class name...
        functionToOverrideName = _functionToOverrideName.Remove(_functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

        //Get the class name...
        functionToOverrideName = _functionToOverrideName.Remove(_functionToOverrideName.Length - functionToOverrideNameEnd.Length - 1);

        //Convert the string into a class type & Grab the methodinfo...
        MethodInfo _functionToOverride = Type.GetType(functionToOverrideName).GetMethod(functionToOverrideNameEnd, (isPublic ? BindingFlags.Public : BindingFlags.NonPublic)
                                                                                                                 | (isInstanceOrVirtual ? BindingFlags.Instance : BindingFlags.Static)
                                                                                                                 | BindingFlags.FlattenHierarchy,
                                                                                                                 null, CallingConventions.Any, parametersForFunction, null);

        if (_functionToOverride == null)
        {
            Console.WriteLine("NO functionToOverride - 404 ERROR...");
            return 404;
        }

        //Get OnCalledGate()
        Type eventMethodType   = Type           .GetType  (MethodInfo.GetCurrentMethod().DeclaringType.FullName);
        MethodInfo eventMethod = eventMethodType.GetMethod("OnCalledGate");

        if (eventMethod == null)
        {
            Console.WriteLine("We failed to get the eventMethod...");
            return 404;
        }

        //Get where the function is...
        int _Source_Base      = _functionToOverride.MethodHandle.GetFunctionPointer().ToInt32();
        int _Destination_Base = eventMethod        .MethodHandle.GetFunctionPointer().ToInt32();

        //Pass forward method locations!
        Source_Base      = _Source_Base;
        Destination_Base = _Destination_Base;

        //We now got the data we need
        allowedToInit = true;
        return 0;
    }

    private static int Source_Base, Destination_Base;

    unsafe static public int Init()
    {
        if (inited == true)
        {
            Console.WriteLine("Already activated...");
            return 0;
        }

        if (allowedToInit == false)
        {
            Console.WriteLine("I say your giving me invalid/no info");
            return -1;
        }

        //Calculate the diffrence between the 2 function's locations
        int   offset_raw         = Destination_Base - Source_Base;
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
