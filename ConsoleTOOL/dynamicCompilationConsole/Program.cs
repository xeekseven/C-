using Microsoft.CSharp;
using System;
using System.Reflection;
using System.Globalization;  
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dynamicCompilationConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            CSharpCodeProvider csPrivoder = new CSharpCodeProvider();

            //ICodeCompiler icCompiler =csPrivoder.CreateCompiler();

            CompilerParameters cParam = new CompilerParameters();
            cParam.ReferencedAssemblies.Add("System.dll");
            cParam.GenerateExecutable = false;
            cParam.GenerateInMemory = true;
            //cParam.OutputAssembly = @"D:\out.dll";
            //CompilerResults cr = icCompiler.CompileAssemblyFromSource(cParam, GenerateCode());
            CompilerResults cr = csPrivoder.CompileAssemblyFromSource(cParam, GenerateCode());

            if(cr.Errors.HasErrors)
            {
                Console.WriteLine("编译错误:");
                foreach(CompilerError err in cr.Errors)
                {
                    Console.WriteLine(err.ErrorText);
                }
            }
            else
            {
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                MethodInfo mi = objHelloWorld.GetType().GetMethod("OutPut");
                Console.WriteLine(mi.Invoke(objHelloWorld,null));

                Console.ReadLine();
            }
        }
        static string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();  
              sb.Append("using System;");  
              sb.Append(Environment.NewLine);  
              sb.Append("namespace DynamicCodeGenerate");  
              sb.Append(Environment.NewLine);  
              sb.Append("{");  
              sb.Append(Environment.NewLine);  
              sb.Append("      public class HelloWorld");  
              sb.Append(Environment.NewLine);  
              sb.Append("      {");  
              sb.Append(Environment.NewLine);  
              sb.Append("          public string OutPut()");  
              sb.Append(Environment.NewLine);  
              sb.Append("          {");  
              sb.Append(Environment.NewLine);  
              sb.Append("               return \"Hello world!\";");  
              sb.Append(Environment.NewLine);  
              sb.Append("          }");  
              sb.Append(Environment.NewLine);  
              sb.Append("      }");  
              sb.Append(Environment.NewLine);  
              sb.Append("}");  
            string code = sb.ToString();

            Console.WriteLine(code);
            return code;
        }
    }
}
