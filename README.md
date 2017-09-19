# Calling C++ from .Net Core
This demonstrates how to create a native, unmanaged library in C++ and invoke
it's functionality from a .Net Core Application. This application just creates a notice box, 
but you could call any function in this manner.

[I wrote an article on medium explaining this]https://blog.quickbird.uk/calling-c-from-net-core-759563bab75d)

## Invoking native code
[Guidance from Microsoft](https://docs.microsoft.com/en-us/dotnet/standard/native-interop)
You can follow those guides, and withing 3 lines of code you’ll be able to summon a UI element using the native Win32 API
```C#
//Define external function to call
[DllImport("user32.dll")]
public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

public static void Main(string[] args) {
// Invoke the function as a regular managed method.
MessageBox(IntPtr.Zero, "Command-line message box", "Attention!", 0);
}
```
![Screenshot](/Images/Screenshot.png)
Note that the `[DllImport("user32.dll")]` part can be used as a normal filepath,
to point directly at any dll you want to use.

## Creating the DLL
I was looking around for the simplest way to create a DLL that can be used this way.
 Choose Empty general C++ project in visual studio.
 ![Screenshot](/Images/CreateDLLProject.png)
 Go to project properties and change the output type to a dynamic link library
 ![Screenshot](/Images/SetDynamicLibrary.png)
We will imagine our DLL is just a collection of functions, so let’s remove an entry point. 
That way compiler won’t frow a hissie fit when it does not find the main() function.
 ![Screenshot](/Images/SetNoMain.png) 
 We will be quick and dirty, and define the function in a header file. 
 Add a header file with the following content:
 
 ```C++
 #ifndef MATH_HPP
#define MATH_HPP
extern "C"
{
    __declspec(dllexport) int __stdcall math_add(int a, int b) {
        return a + b;
    }
}

#endif
```
Add a .cpp file with ' #include "math.h" ' Without it, the compiler will ignore the header file.
Compile the DLL in release mode. **It is important to choose x64** if your computer supports it. 
.Net Core will run in 64 bit mode, and if the dll is 32 bit, using it will fail! Note the location 
where the dll got saved.

## Using the DLL
using the DLL is quite straight-forward. Create a .Net Core console application, and insert this code:
```C#
using System.Runtime.InteropServices;
namespace NetCore
{
  public class Program 
  {
   // Insert correct filePath
      [DllImport(@"C:\...\WindowsNative.dll", EntryPoint = 
       "math_add", CallingConvention = CallingConvention.StdCall)]
     public static extern int Add(int a, int b);
     public static void Main(string[] args)
     {
        int result = Add(1 ,2);
        Console.WriteLine("result is {0}", result);
        //Halts the program
        Console.ReadKey();
      }
   }
}
```

Next Steps
I am yet to get around to testing this with linux. 
There is a fairly in-depth guide to [writing ddl’s for linux by microsoft](https://blogs.msdn.microsoft.com/vcblog/2016/03/30/visual-c-for-linux-development/).
 Note that the DLLs will have a different file extension — .so instead of .dll, and that needs to be managed.