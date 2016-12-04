using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace NetCore
{
    public class Program
    {

        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [DllImport("user32.dll")]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

        //Write-to-console function
        [DllImport("msvcrt.dll")]
        public static extern int puts(string c);


        // Import our own DLL. This should point to where-ever the the DLL is. 
        [DllImport(@"C:\Development\NetCoreNativeCall\x64\Release\WindowsNative.dll", EntryPoint = "math_add", CallingConvention = CallingConvention.StdCall)]
        public static extern int Add(int a, int b);

        public static void Main(string[] args)
        {
            //creates messagebox.
            MessageBox(IntPtr.Zero, "Command-line message box", "Attention!", 0);
            //Writes to console
            puts("test");
            //Calls our own dll!
            int result = Add(1 ,2);
            Console.WriteLine("result is {0}", result);
            //Halts the program
            Console.ReadKey(); 
        }
    }
}
