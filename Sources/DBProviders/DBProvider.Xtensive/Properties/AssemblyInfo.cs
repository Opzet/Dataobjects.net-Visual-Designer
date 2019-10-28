using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using TXSoftware.DataObjectsNetEntityModel.DBProvider;
using TXSoftware.DataObjectsNetEntityModel.DBProvider.Xtensive;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DataObjects.Net Entity Model Designer Xtensive Database Provider")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("TX Software")]
[assembly: AssemblyProduct("DataObjectsNetEntityModel.DBProvider.Xtensive")]
[assembly: AssemblyCopyright("Copyright © TX Software 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("06e2e95d-e7f4-4d9a-993a-bf74f49238c0")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.4.0")]

[assembly: DBProviderModuleRegister(typeof(DBProviderModule))]

[assembly: InternalsVisibleTo(@"TXSoftware.DataObjectsNetEntityModel.DBProvider.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100e1e076af5277c59d2f5e791bff6a7e60f1f4b11c094ad2a6de6420f869ae3cf8d7479717d86e8dfc60e5404c223237000fd7c31a308fa8e70b0f7b3a17ee2d28783c68ab2c0d081d6651ef7d8089fd93d6d900a68d52b6f09a6627865f1c4415fcdcb9f3e27e474f9714e67957f73292dfc114fdeff93cb41dc5de35e5069fd9")]