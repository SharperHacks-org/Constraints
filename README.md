![SharperHacks logo](SHLLC-Logo.jpg)
# Constraints Library for .NET
## SharperHacks.CoreLibs.Constraints

Defines classes and methods to verify runtime contraints are met.

Licensed under the Apache License, Version 2.0. See [LICENSE](LICENSE).

Contact: joseph@sharperhacks.org

Nuget: https://www.nuget.org/packages/SharperHacks.CoreLibs.Constraints

### Targets
- net7.0
- net8.0

### Classes

#### Verify
A static class, containing static methods to verify constraints. Unlike Assert,
these methods are compiled and fully functional in production as well as DEBUG
code. All methods include caller member name, caller file path and caller line
number in the diagnostic message suppled to the VerifyException that is thrown when
constraints are not met.

##### Examples:
```
    Verify.IsNotNullOrEmpty(str);
    Verify.IsLessThan(X, Y, "X must be less than Y");
```

#### VerifyException
The exception class that is thrown by Verify methods when a constraint is not met.
Provides a consitently formatted exception string for all constraint failures.

