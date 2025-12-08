using System.Runtime.InteropServices;

// Not going to support COM interop.
[assembly: ComVisible(false)]

[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)]
