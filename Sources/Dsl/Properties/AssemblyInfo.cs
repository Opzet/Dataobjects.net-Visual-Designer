#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("DataObjects.Net Entity Model Designer")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("TX Software")]
[assembly: AssemblyProduct("TXSoftware.DataObjectsNetEntityModel.Dsl")]
[assembly: AssemblyCopyright("Copyright © TX Software 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("1.0.5.0")]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

//
// Make the Dsl project internally visible to the DslPackage assembly
//
[assembly: InternalsVisibleTo(@"TXSoftware.DataObjectsNetEntityModel.DslPackage, PublicKey=0024000004800000940000000602000000240000525341310004000001000100E1E076AF5277C59D2F5E791BFF6A7E60F1F4B11C094AD2A6DE6420F869AE3CF8D7479717D86E8DFC60E5404C223237000FD7C31A308FA8E70B0F7B3A17EE2D28783C68AB2C0D081D6651EF7D8089FD93D6D900A68D52B6F09A6627865F1C4415FCDCB9F3E27E474F9714E67957F73292DFC114FDEFF93CB41DC5DE35E5069FD9")]

[assembly: InternalsVisibleTo(@"TXSoftware.DataObjectsNetEntityModel.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100E1E076AF5277C59D2F5E791BFF6A7E60F1F4B11C094AD2A6DE6420F869AE3CF8D7479717D86E8DFC60E5404C223237000FD7C31A308FA8E70B0F7B3A17EE2D28783C68AB2C0D081D6651EF7D8089FD93D6D900A68D52B6F09A6627865F1C4415FCDCB9F3E27E474F9714E67957F73292DFC114FDEFF93CB41DC5DE35E5069FD9")]