using System;
using FunctionJumper;
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
        static public int DAPI()
        {
            Console.WriteLine("D API");
            return -89/3;
        }
    }
}

namespace Function_Jumping_Example
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            int[] num = new int[5] { 5, 5, 5, 5, 5 };
            int*[] num2 = new int*[1] { (int*)0 };

            //Initial try
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9));
            Console.WriteLine(Function_Far_Far_Away.RimAPIFunctions.DAPI());

            Console.WriteLine("HIT ENTER!!! \n\r \n\r");
            Console.ReadLine();

            MethodInfo[] overrideMethods = {
                typeof(Function_Far_Far_Away.RimAPIFunctions).GetMethod("AAPI" , BindingFlags.NonPublic | BindingFlags.Static),
                typeof(Function_Far_Far_Away.RimAPIFunctions).GetMethod("BAPI"),
                typeof(Program)                              .GetMethod("C"    , BindingFlags.NonPublic | BindingFlags.Static, null, CallingConventions.Any, new Type[1] { typeof(int) }, null),
                typeof(Function_Far_Far_Away.RimAPIFunctions).GetMethod("DAPI" , BindingFlags.Public    | BindingFlags.Static)
            };

            MethodInfo[] overridingMethods = {
                typeof(RimFunctions).GetMethod("A"),
                typeof(RimFunctions).GetMethod("B"),
                typeof(RimFunctions).GetMethod("C" , BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, new Type[1] { typeof(int) }, null),
                typeof(RimFunctions).GetMethod("D")
            };

            //init functions
            Console.WriteLine(EventMaster.OverrideFrom(overrideMethods[0],
                                                       "public static void OnCalledG(){if(OnCalled != null){OnCalled();}return;}", 
                                                       "void", "", "",                          
                                                       new string[0] { }));

            Console.WriteLine(EventMaster.OverrideFrom(overrideMethods[1],                                               
                                                       "public static unsafe void OnCalledG(int[] num, ref int*[] num2){if(OnCalled != null){OnCalled(num, ref num2);}return;}",
                                                       "void",  "", "int[] num, ref int*[] num2", 
                                                       new string[0] { }));

            Console.WriteLine(EventMaster.OverrideFrom(overrideMethods[2],                                                  
                                                       "public static int  OnCalledG(int num, int num2){if(OnCalled != null){return OnCalled(num);}return -1;}", 
                                                       "int",  "", "int num",          
                                                       new string[0] { }));

            Console.WriteLine(EventMaster.OverrideFrom(overrideMethods[3],
                                                       "public static int  OnCalledG(){if(OnCalled != null){return OnCalled();}return -1;}",
                                                       "int", "", "",
                                                       new string[0] { }) + "\n\r");

            //Add them once!
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[0])).AddSub(overridingMethods[0]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[1])).AddSub(overridingMethods[1]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[2])).AddSub(overridingMethods[2]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[3])).AddSub(overridingMethods[3]) + "\n\r");

            //Now we got our overriden methods... YAY!
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9));;
            Console.WriteLine(Function_Far_Far_Away.RimAPIFunctions.DAPI());

            Console.WriteLine("HIT ENTER!!!");
            Console.ReadLine();

            //Lets have them each called twice!
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[0])).AddSub(overridingMethods[0]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[1])).AddSub(overridingMethods[1]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[2])).AddSub(overridingMethods[2]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[3])).AddSub(overridingMethods[3]) + "\n\r");

            //Everyhing now runs twice...
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9)); ;
            Console.WriteLine(Function_Far_Far_Away.RimAPIFunctions.DAPI());

            Console.WriteLine("HIT ENTER!!!");
            Console.ReadLine();

            //Lets remove them once... now thier only called once...
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[0])).RemoveSub(overridingMethods[0]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[1])).RemoveSub(overridingMethods[1]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[2])).RemoveSub(overridingMethods[2]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[3])).RemoveSub(overridingMethods[3]) + "\n\r");

            //Should work as expected...
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9)); ;
            Console.WriteLine(Function_Far_Far_Away.RimAPIFunctions.DAPI());

            Console.WriteLine("HIT ENTER!!!");
            Console.ReadLine();

            //We will refuse to allow you to remove them now... we can't have less than 1 sub... (I don't trust the User Implemented OnCalledG method!)
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[0])).RemoveSub(overridingMethods[0]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[1])).RemoveSub(overridingMethods[1]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[2])).RemoveSub(overridingMethods[2]));
            Console.WriteLine(EventMaster.findEvent(EventMaster.getName(overrideMethods[3])).RemoveSub(overridingMethods[3]) + "\n\r");

            //Just like the last,,,
            Function_Far_Far_Away.RimAPIFunctions.AAPI();
            Function_Far_Far_Away.RimAPIFunctions.BAPI(num, ref num2);
            Console.WriteLine(C(9)); ;
            Console.WriteLine(Function_Far_Far_Away.RimAPIFunctions.DAPI());

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
        static public int D()
        {
            Console.WriteLine("D NORM");
            return -89/2;
        }
    }
}