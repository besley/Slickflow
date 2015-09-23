Oracle.ManagedDataAccess Nuget Package Version 12.1.022 README
==============================================================

Release 12.1.0.2.2 for RDBMS 12.1.0.2 Bundle Patch 6 Production

Release Notes: Oracle Data Provider for .NET, Managed Driver

June 2015

Copyright (c) Oracle Corporation 2015

This document provides information that supplements the Oracle Data Provider for .NET (ODP.NET) documentation. 
You have downloaded Oracle Data Provider for .NET from Oracle, the license agreement to which is available at 
http://www.oracle.com/technetwork/licenses/distribution-license-152002.html

TABLE OF CONTENTS
*New Features
*Installation and Configuration Steps
*Installation Changes
*Documentation Corrections and Additions
*Fixed Bugs Since Last ODP.NET NuGet Release
*ODP.NET, Managed Driver Tips, Limitations, and Known Issues

Note: The 32-bit "Oracle Developer Tools for Visual Studio" download from http://otn.oracle.com/dotnet is 
required for Entity Framework design-time features and for other Visual Studio designers such as the 
TableAdapter Wizard. This NuGet download does not enable design-time tools; it only provides run-time support. 
This version of ODP.NET supports Oracle Database version 10.2 and higher.



 New Features since Oracle.ManagedDataAccess Nuget Package Version 12.1.021
===========================================================================
1. ODP.NET, Managed Driver - Microsoft Local Security Authority (MSLSA)
ODP.NET, Managed Driver now supports the Kerberos credential cache type, MSLSA. MSLSA is used to access the 
Microsoft Kerberos Logon Session credentials cache.

2. ODP.NET, Managed Driver - SSL/TLS Connections Use a Single Port
An ODP.NET, Managed Driver SSL/TLS connection will now continue on the original connection to the database 
listener instead of the previous SSL/TLS client redirection to a database server created new listening 
endpoint on a dynamic (ephemeral) port. Hence, firewalls will only need to allow access to the TNS listener's 
port. For example, 1521.



Installation and Configuration Steps
====================================
The downloads are NuGet packages that can be installed with the NuGet Package Manager. These instructions apply 
to install ODP.NET, Managed Driver.

1. Un-GAC any existing ODP.NET 12.1.0.2 versions you have installed. For example, if you plan to use only the 
ODP.NET, Managed Driver, only un-GAC existing managed ODP.NET 12.1.0.2 versions then.

2. In Visual Studio 2010, 2012, or 2013, open NuGet Package Manager from an existing Visual Studio project. 

3. Install the NuGet package from an OTN-downloaded local package source or from nuget.org.


   From Local Package Source
   -------------------------
   A. Click on the Settings button in the lower left of the dialog box.

   B. Click the "+" button to add a package source. In the Source field, enter in the directory location where the 
   NuGet package(s) were downloaded to. Click the Update button, then the Ok button.

   C. On the left side, under the Online root node, select the package source you just created. The ODP.NET NuGet 
   packages will appear.


   From Nuget.org
   --------------
   A. In the Search box in the upper right, search for the package with id, "Oracle.ManagedDataAccess". Verify 
   that the package uses this unique ID to ensure it is the offical Oracle Data Provider for .NET, Managed Driver 
   download.

   B. Select the package you wish to install.


4. Click on the Install button to select the desired NuGet package(s) to include with the project. Accept the 
license agreement and Visual Studio will continue the setup.

5. Open the app/web.config file to configure the ODP.NET connection string and local naming parameters 
(i.e. tnsnames.ora). Below is an example of configuring the local naming parameters:

  <oracle.manageddataaccess.client>
    <version number="*">
      <dataSources>
        <!-- Customize these connection alias settings to connect to Oracle DB -->
        <dataSource alias="MyDataSource" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL))) " />
      </dataSources>
    </version>
  </oracle.manageddataaccess.client>

After following these instructions, ODP.NET is now configured and ready to use.

NOTE: ODP.NET, Managed Driver comes with two platform specific assemblies:

        i.  Oracle.ManagedDataAccessDTC.dll (for Distributed Transaction Support)
        ii. Oracle.ManagedDataAccessIOP.dll (for Kerberos Support)

The Oracle.ManagedDataAccessDTC.dll assembly is ONLY needed if you are using Distributed Trasactions and the 
.NET Framework being used is 4.5.1 or lower. If you are using .NET Framework 4.5.2 or higher, this assembly does 
not need to be referenced by your application.

The Oracle.ManagedDataAccessIOP.dll assembly is ONLY needed if you are using Kerberos. Kerberos users will need 
to download MIT Kerberos for Windows 4.0.1 or higher from 
  http://web.mit.edu/kerberos/dist/ 
to utilize ODP.NET, Managed Driver's support of Kerberos.

These asssemblies are located under
      packages\Oracle.ManagedDataAccess.12.1.022\bin\x64
and
      packages\Oracle.ManagedDataAccess.12.1.022\bin\x86
depending on the platform.

If these assemblies are required by your application, your Visual Studio project requires additional changes.

Use the following steps for your application to use the 64-bit version of Oracle.ManagedDataAccessDTC.dll:

1. Right click on the Visual Studio project.
2. Select Add -> New Folder
3. Name the folder x64.
4. Right click on the newly created x64 folder
5. Select Add -> Existing Item
6. Browse to packages\Oracle.ManagedDataAccess.12.1.022\bin\x64 under your project solution directory.
7. Choose Oracle.ManagedDataAccessDTC.dll
8. Click the 'Add' button
9. Left click the newly added Oracle.ManagedDataAccessDTC.dll in the x64 folder
10. In the properties window, set 'Copy To Output Directory' to 'Copy Always'.

For x86 targeted applications, name the folder x86 and add assemblies from the 
packages\Oracle.ManagedDataAccess.12.1.022\bin\x86 folder.

Use the same steps for adding Oracle.ManagedDataAccessIOP.dll.

To make your application platform independent even if it depends on Oracle.ManagedDataAccessDTC.dll and/or 
Oracle.ManagedDataAccessIOP.dll, create both x64 and x86 folders with the necessary assemblies added to them.



Installation Changes
====================
The following app/web.config entries are added by including the "Official Oracle ODP.NET, Managed Driver" NuGet package 
to your application:

1) Configuration Section Handler

The following entry is added to the app/web.config to enable applications to add an <oracle.manageddataaccess.client> 
section for ODP.NET, Managed Driver-specific configuration:

<configuration>
  <configSections>
    <section name="oracle.manageddataaccess.client" type="OracleInternal.Common.ODPMSectionHandler, Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />
  </configSections>
</configuration>

Note: If your application is a web application and the above entry was added to a web.config and the same config 
section handler for "oracle.manageddataaccess.client" also exists in machine.config but the "Version" attribute values 
are different, an error message of "There is a duplicate 'oracle.manageddataaccess.client' section defined." may be 
observed at runtime.  If so, the config section handler entry in the machine.config for 
"oracle.manageddataaccess.client" has to be removed from the machine.config for the web application to not encounter 
this error.  But given that there may be other applications on the machine that depended on this entry in the 
machine.config, this config section handler entry may need to be moved to all of the application's .NET config file on 
that machine that depend on it.

2) DbProviderFactories

The following entry is added for applications that use DbProviderFactories and DbProviderFactory classes. Also, any 
DbProviderFactories entry for "Oracle.ManagedDataAccess.Client" in the machine.config will be ignored with the following 
entry:

<configuration>
  <system.data>
    <DbProviderFactories>
      <remove invariant="Oracle.ManagedDataAccess.Client" />
      <add name="ODP.NET, Managed Driver" invariant="Oracle.ManagedDataAccess.Client" description="Oracle Data Provider for .NET, Managed Driver" type="Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342" />
    </DbProviderFactories>
  </system.data>
</configuration>

3) Dependent Assembly

The following entry is created to ignore policy DLLs for Oracle.ManagedDataAccess.dll and always use the 
Oracle.ManagedDataAccess.dll version that is specified by the newVersion attribute in the <bindingRedirect> element.  
The newVersion attribute corresponds to the Oracle.ManagedDataAccess.dll version which came with the NuGet package 
associated with the application.

<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <publisherPolicy apply="no" />
        <assemblyIdentity name="Oracle.ManagedDataAccess" publicKeyToken="89b483f429c47342" culture="neutral" />
        <bindingRedirect oldVersion="4.121.0.0 - 4.65535.65535.65535" newVersion="4.121.2.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>

4) Data Sources

The following entry is added to provide a template on how a data source can be configured in the app/web.config. 
Simply rename "MyDataSource" to an alias of your liking and modify the PROTOCOL, HOST, PORT, SERVICE_NAME as required 
and un-comment the <dataSource> element. Once that is done, the alias can be used as the "data source" attribute in 
your connection string when connecting to an Oracle Database through ODP.NET, Managed Driver.

<configuration>
  <oracle.manageddataaccess.client>
    <version number="*">
      <dataSources>
        <dataSource alias="SampleDataSource" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL))) " />
      </dataSources>
    </version>
  </oracle.manageddataaccess.client>
</configuration>



Documentation Corrections and Additions
=======================================
This section contains information that corrects or adds to existing ODP.NET documentation, which can be found here:
http://docs.oracle.com/cd/E56485_01/index.htm


ODP.NET, Managed Driver Support for Oracle Database 12c Implicit Ref Cursor 
---
ODP.NET, Managed Driver introduces support for the new Oracle Database 12c Implicit Ref Cursor. Configuration occurs 
using the <implicitrefcursor> .NET configuration section. When using database implicit ref cursors, the bindInfo element 
should be specified with a mode of "Implicit": 

<bindinfo mode="Implicit" /> 

For additional information refer to the implicitRefCursor section in Chapter 2 of the Oracle Data Provider for .NET 
Developer's Guide. 


Session Time Zone Hour Offset in ODP.NET Managed and Unmanaged Drivers 
---
ODP.NET managed and unmanaged drivers set the default session time zone differently. While the session time zone for 
unmanaged ODP.NET uses an hour offset, managed ODP.NET uses the region identifier for setting its session time zone. 
As a result, managed ODP.NET is sensitive to daylight savings in scenarios where the timestamp LTZ values have to be 
converted from/to the session time zone.

There are two methods to resolve this difference if needed. For ODP.NET, Unmanaged Driver, the application explicitly 
sets the region identifier with the environment variable 'ORA_SDTZ' (e.g. 'set ORA_SDTZ = <Region ID>'). If ORA_SDTZ 
variable is set, Oracle Client considers this value as the session time zone. The second method is to execute an alter 
session command to set the session time zone property to the region identifier. 


ODP.NET, Managed Driver with NTS Authentication 
---
ODP.NET, Managed Driver supports NTS authentication to the database, except when the Windows domain is constrained to 
only support Kerberos-based domain authentication. 


ODP.NET, Managed Driver SSL Connections with Firewalls 
---
ODP.NET, Managed Driver SSL connections require a redirect to a dynamic port on the database server side. If a firewall 
exists between the database client and server, then all firewall ports must be enabled or the dynamic firewall port 
Oracle chooses must be enabled at run-time.

 

Fixed Bugs Since Last ODP.NET NuGet Release
===========================================
This section list bugs that have been fixed since the last ODP.NET NuGet release.

BUG 20361140 - ORA-01461: CAN BIND A LONG VALUE ONLY FOR INSERT INTO A LONG COLUMN
BUG 20401862 - ODP MANAGED DRIVER APPLICATION FAILS TO CONNECT TO RAC DB USING OID 
BUG 20460558 - BINDBYNAME CAUSES QUERY TO HANG 
BUG 19261209 - FILE WALLET WITH MANAGED ODP.NET PROVIDER FAILS WITH ORA-01017 
BUG 20419677 - SLOW OPENING CONNECTION WHEN EZCONNECT AND "WORLD" SETTINGS USED TOGETHER 
BUG 20206306 - ORA-01483 WHEN BINDING NUMBER AND NCLOB > 4K CHARACTERS 



ODP.NET, Managed Driver Tips, Limitations, and Known Issues
===========================================================
This section contains information that is specific to ODP.NET, Managed Driver. 

1. OracleConnection object's OpenWithNewPassword() method invocation will result in an ORA-1017 error with 11.2.0.3.0 
and earlier versions of the database. [Bug 12876992]

2. Stored functions/procedures in a PDB cannot be added to a .NET Entity Framework model. [Bug 17344899]
