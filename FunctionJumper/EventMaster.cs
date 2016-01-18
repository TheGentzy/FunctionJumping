using System;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom.Compiler;

namespace FunctionJumper
{
    public static class EventMaster
    {
        static List<MethodInfo> overriddenMethods = new List<MethodInfo>();
        static List<EventInterface> events = new List<EventInterface>();

        static public EventInterface findEvent(string _funcName)
        {
            foreach (EventInterface e in events)
            {
                if (e.funcName == _funcName)
                {
                    return e;
                }
            }
            return null;
        }

        static public void addOverriddenMethod(MethodInfo overridenMethod)
        {
            overriddenMethods.Add(overridenMethod);
        }

        static public int OverrideFrom(MethodInfo sourceMethod, string OnCalledG, string returnType, string usingDirectives, string args, string[] assemblyNeeded)
        {
            if (sourceMethod == null)
            {
                return (int)ReturnCodes.NoSource;
            }

            bool makeNewMethod = true;
            foreach (MethodInfo methodInfo in overriddenMethods)
            {
                if (methodInfo == sourceMethod)
                {
                    makeNewMethod = false;
                    break;
                }
            }

            //make function name
            string funcName = getName(sourceMethod).Replace('.', '_');

            if (makeNewMethod)
            {
                //We are gonna compile code at runtime... YAY!
                //Interface for compiler
                CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("CSharp");

                CompilerParameters compilerParameters = new CompilerParameters();

                // Generate a class library 
                compilerParameters.GenerateExecutable = false;

                // Generate debug information.
                compilerParameters.IncludeDebugInformation = true;

                // Add an assembly reference.
                foreach (String assemblyReference in assemblyNeeded)
                {
                    compilerParameters.ReferencedAssemblies.Add(assemblyReference);
                }

                // Don't save the assembly as a physical file.
                compilerParameters.GenerateInMemory = true;

                // Set the level at which the compiler 
                // should start displaying warnings.
                compilerParameters.WarningLevel = 3;

                // Set whether to treat all warnings as errors.
                compilerParameters.TreatWarningsAsErrors = false;

                // Set compiler argument to optimize output.
                compilerParameters.CompilerOptions = "/optimize /unsafe";

                //Source  
                string sourceCode = "using System;\n\r" +
                                        "using System.Reflection;\n\r" +
                                        "__USINGDIRECTIVES__\n\r" +
                                        "\n\r" +
                                        "namespace FunctionJumper\n\r" +
                                        "{\n\r" +
                                        "    static public class EventFor___FUNCNAME__\n\r" +
                                        "    {\n\r" +
                                        "        public__UNSAFE?__ delegate __RETURNTYPE__ Function (__ARGS__);\n\r" +
                                        "        static public__UNSAFE?__ event Function OnCalled;\n\r" +
                                        "\n\r" +
                                        "public static MethodInfo eventMethod = typeof(Function).GetMethod(\"Invoke\");\n\r" +
                                        "\n\r" +
                                        "__OnCalledG__\n\r" +
                                        "\n\r" +
                                        "        static public unsafe int InitEvent(MethodInfo functionToOverride)\n\r" +
                                        "        {\n\r" +
                                        "            Type eventMethodType = Type.GetType(MethodInfo.GetCurrentMethod().DeclaringType.FullName);\n\r" +
                                        "            MethodInfo eventMethod = eventMethodType.GetMethod(\"OnCalledG\");\n\r" +
                                        "\n\r" +
                                        "            //Get where the function is...\n\r" +
                                        "            int Source_Base = functionToOverride.MethodHandle.GetFunctionPointer().ToInt32();\n\r" +
                                        "            int Destination_Base = eventMethod.MethodHandle.GetFunctionPointer().ToInt32();\n\r" +
                                        "\n\r" +
                                        "\n\r" +
                                        "            //Calculate the diffrence between the 2 function's locations\n\r" +
                                        "            int offset_raw = Destination_Base - Source_Base;\n\r" +
                                        "            uint* Pointer_Raw_Source = (uint*)Source_Base;\n\r" +
                                        "\n\r" +
                                        "            // [WEIRD POINTER MATH] //\n\r" +
                                        "            //From RawCode\n\r" +
                                        "            *(Pointer_Raw_Source + 0) = 0xE9909090;\n\r" +
                                        "            *(Pointer_Raw_Source + 1) = (uint)(offset_raw - 8);\n\r" +
                                        "            // [/WEIRD POINTER MATH] //\n\r" +
                                        "\n\r" +
                                        "            return 0;\n\r" +
                                        "        }\n\r" +
                                        "\n\r" +
                                        "    }\n\r" +
                                        "}\n\r";

                //Fill out template.
                sourceCode = sourceCode.Replace("__FUNCNAME__", funcName);
                sourceCode = sourceCode.Replace("__ARGS__", args);
                sourceCode = sourceCode.Replace("__OnCalledG__", OnCalledG);
                sourceCode = sourceCode.Replace("__RETURNTYPE__", returnType);
                sourceCode = sourceCode.Replace("__USINGDIRECTIVES__", usingDirectives);

                if (args.Contains("*"))
                {
                    sourceCode = sourceCode.Replace("__UNSAFE?__", " unsafe");
                }
                else
                {
                    sourceCode = sourceCode.Replace("__UNSAFE?__", "");
                }

                //Lets compile!
                CompilerResults compilerResults = codeDomProvider.CompileAssemblyFromSource(compilerParameters, sourceCode);

                // Write the string to a file.
                if (System.IO.Directory.Exists("Code/") == false)
                {
                    System.IO.Directory.CreateDirectory("Code/");
                }

                System.IO.StreamWriter file = new System.IO.StreamWriter("Code/CODE FOR " + funcName + ".txt");
                file.WriteLine(sourceCode);
                file.Close();

                if (compilerResults.Errors.Count > 0)
                {
                    Console.WriteLine(funcName + " FAILED DUE TO COMPILE ISSUE. SEE 'Code/CODE FOR " + funcName + ".txt'\n\r\n\r");
                    Console.WriteLine("ERRORS:\n\r");

                    //Print errors...
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        Console.WriteLine("  {0}", compilerError.ToString());
                        Console.WriteLine("  Line: {0} Collum: {1}", new object[] { compilerError.Line, compilerError.Column });
                        Console.WriteLine();
                    }
                    return (int)ReturnCodes.CompileError;
                }

                Assembly compiledAssembly = compilerResults.CompiledAssembly;
                Type type = compiledAssembly.GetType("FunctionJumper.EventFor___FUNCNAME__".Replace("__FUNCNAME__", funcName));
                MethodInfo delegateMethod = (MethodInfo)type.GetField("eventMethod").GetValue(null);

                if (type == null)
                {
                    return (int)ReturnCodes.NoTypeForEvent;
                }

                //We will make a new interface now
                EventInterface eventInterface = new EventInterface();
                events.Add(eventInterface);

                //Line ends here, now the interface specific implementation.
                return eventInterface.InitINTERNAL(sourceMethod, type, delegateMethod, getName(sourceMethod));
            }
            return (int)ReturnCodes.Success;
        }

        public static string getName(MethodInfo method)
        {
            if (method == null)
            {
                return null;
            }
            return method.DeclaringType.FullName + "." + method.Name;
        }
    }
}