# Minimal repro of clrmd exception

```
> dotent build
> ./bin/Debug/net7.0-macos/osx-arm64/test_app.app/Contents/MacOS/test_app
Working thread...
Working thread...
Unhandled exception. Microsoft.Diagnostics.Runtime.ClrDiagnosticsException: Could not attach to process 55656, errno: 1
   at Microsoft.Diagnostics.Runtime.MacOS.MacOSProcessDataReader..ctor(Int32 processId, Boolean suspend)
   at Microsoft.Diagnostics.Runtime.DataTarget.AttachToProcess(Int32 processId, Boolean suspend)
   at Program.Main() in /Users/n8ta/SIG8/test_app/Program.cs:line 41
```
