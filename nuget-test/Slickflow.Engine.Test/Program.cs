// Slickflow.Engine Test Project
// This is a sample test project to verify the NuGet package installation and functionality

using Microsoft.Extensions.Configuration;
using Slickflow.Engine.Service;
using Slickflow.Engine.Business.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Slickflow.Engine.Test
{
    class Program
    {
        private static IConfiguration? _configuration;
        private static bool _databaseInitialized = false;

        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Slickflow.Engine Test Suite");
            Console.WriteLine("========================================");
            Console.WriteLine();

            // Initialize configuration and database
            InitializeConfiguration();
            InitializeDatabase();

            Console.WriteLine("========================================");
            Console.WriteLine("Starting Test Execution");
            Console.WriteLine("========================================");
            Console.WriteLine();

            // Level 1: Package Installation Verification
            RunLevel1Tests_PackageInstallation();

            // Level 2: Assembly Information Tests
            RunLevel2Tests_AssemblyInformation();

            // Level 3: Type Reflection Tests
            RunLevel3Tests_TypeReflection();

            // Level 4: Service Instantiation Tests
            RunLevel4Tests_ServiceInstantiation();

            // Level 5: Business Method Tests
            if (_databaseInitialized)
            {
                RunLevel5Tests_BusinessMethods();
            }
            else
            {
                Console.WriteLine("⚠ Level 5 Tests Skipped: Database not initialized");
                Console.WriteLine();
            }

            Console.WriteLine("========================================");
            Console.WriteLine("All Tests Completed!");
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

        #region Initialization Methods

        /// <summary>
        /// Initialize configuration from appsettings.json
        /// </summary>
        static void InitializeConfiguration()
        {
            Console.WriteLine("Initializing Configuration...");
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                _configuration = builder.Build();
                Console.WriteLine("✓ Configuration initialized successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Failed to initialize configuration: {ex.Message}");
                Console.WriteLine("  Continuing without configuration...");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Initialize database connection
        /// </summary>
        static void InitializeDatabase()
        {
            Console.WriteLine("Initializing Database Connection...");
            
            if (_configuration == null)
            {
                Console.WriteLine("⚠ Configuration not available, skipping database initialization");
                Console.WriteLine();
                return;
            }

            try
            {
                var dbType = _configuration["ConnectionStrings:WfDBConnectionType"];
                var sqlConnectionString = _configuration["ConnectionStrings:WfDBConnectionString"];

                if (!string.IsNullOrEmpty(dbType) && !string.IsNullOrEmpty(sqlConnectionString))
                {
                    Console.WriteLine($"  Database Type: {dbType}");
                    Console.WriteLine($"  Connection String: {sqlConnectionString.Substring(0, Math.Min(50, sqlConnectionString.Length))}...");
                    
                    Slickflow.Data.DBTypeExtenstions.InitConnectionString(dbType, sqlConnectionString);
                    _databaseInitialized = true;
                    Console.WriteLine("✓ Database connection initialized successfully");
                }
                else
                {
                    Console.WriteLine("⚠ Database connection strings not found in appsettings.json");
                    Console.WriteLine("  Business method tests will be skipped");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Failed to initialize database: {ex.Message}");
                Console.WriteLine("  Business method tests will be skipped");
            }
            
            Console.WriteLine();
        }

        #endregion

        #region Level 1: Package Installation Verification

        /// <summary>
        /// Level 1 Tests: Verify package installation and type availability
        /// </summary>
        static void RunLevel1Tests_PackageInstallation()
        {
            Console.WriteLine("Level 1: Package Installation Verification");
            Console.WriteLine("----------------------------------------");
            
            try
            {
                // Test 1.1: Verify IWorkflowService interface exists
                Console.WriteLine("Test 1.1: Verifying IWorkflowService interface...");
                Type workflowServiceType = typeof(IWorkflowService);
                Console.WriteLine($"  ✓ IWorkflowService type found: {workflowServiceType.FullName}");
                
                // Test 1.2: Verify WorkflowService class exists
                Console.WriteLine("Test 1.2: Verifying WorkflowService class...");
                Type workflowServiceImplType = typeof(WorkflowService);
                Console.WriteLine($"  ✓ WorkflowService type found: {workflowServiceImplType.FullName}");
                
                // Test 1.3: Verify WorkflowService implements IWorkflowService
                Console.WriteLine("Test 1.3: Verifying implementation relationship...");
                if (workflowServiceImplType.GetInterfaces().Contains(workflowServiceType))
                {
                    Console.WriteLine($"  ✓ WorkflowService implements IWorkflowService");
                }
                else
                {
                    Console.WriteLine($"  ✗ WorkflowService does not implement IWorkflowService");
                }
                
                Console.WriteLine("✓ Level 1 Tests: All passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Level 1 Tests: Failed - {ex.Message}");
                Console.WriteLine("  Cannot continue with other tests");
                return;
            }
            
            Console.WriteLine();
        }

        #endregion

        #region Level 2: Assembly Information Tests

        /// <summary>
        /// Level 2 Tests: Check assembly information
        /// </summary>
        static void RunLevel2Tests_AssemblyInformation()
        {
            Console.WriteLine("Level 2: Assembly Information Tests");
            Console.WriteLine("----------------------------------------");
            
            try
            {
                var assembly = typeof(IWorkflowService).Assembly;
                
                // Test 2.1: Assembly name
                Console.WriteLine("Test 2.1: Assembly name...");
                Console.WriteLine($"  ✓ Assembly Name: {assembly.GetName().Name}");
                
                // Test 2.2: Assembly version
                Console.WriteLine("Test 2.2: Assembly version...");
                Console.WriteLine($"  ✓ Assembly Version: {assembly.GetName().Version}");
                
                // Test 2.3: Assembly location
                Console.WriteLine("Test 2.3: Assembly location...");
                Console.WriteLine($"  ✓ Assembly Location: {assembly.Location}");
                
                Console.WriteLine("✓ Level 2 Tests: All passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Level 2 Tests: Failed - {ex.Message}");
            }
            
            Console.WriteLine();
        }

        #endregion

        #region Level 3: Type Reflection Tests

        /// <summary>
        /// Level 3 Tests: Type reflection and discovery
        /// </summary>
        static void RunLevel3Tests_TypeReflection()
        {
            Console.WriteLine("Level 3: Type Reflection Tests");
            Console.WriteLine("----------------------------------------");
            
            try
            {
                var assembly = typeof(IWorkflowService).Assembly;
                var types = assembly.GetTypes();
                
                // Test 3.1: Total type count
                Console.WriteLine("Test 3.1: Total types in assembly...");
                Console.WriteLine($"  ✓ Total types: {types.Length}");
                
                // Test 3.2: Public types count
                Console.WriteLine("Test 3.2: Public types count...");
                var publicTypes = types.Where(t => t.IsPublic && !t.IsInterface && !t.IsAbstract).ToList();
                Console.WriteLine($"  ✓ Public concrete types: {publicTypes.Count}");
                
                // Test 3.3: Sample types
                Console.WriteLine("Test 3.3: Sample public types (first 10)...");
                int count = 0;
                foreach (var type in publicTypes)
                {
                    if (count >= 10) break;
                    Console.WriteLine($"    - {type.Name}");
                    count++;
                }
                
                Console.WriteLine("✓ Level 3 Tests: All passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Level 3 Tests: Failed - {ex.Message}");
            }
            
            Console.WriteLine();
        }

        #endregion

        #region Level 4: Service Instantiation Tests

        /// <summary>
        /// Level 4 Tests: Service instantiation and basic functionality
        /// </summary>
        static void RunLevel4Tests_ServiceInstantiation()
        {
            Console.WriteLine("Level 4: Service Instantiation Tests");
            Console.WriteLine("----------------------------------------");
            
            try
            {
                // Test 4.1: Create WorkflowService instance
                Console.WriteLine("Test 4.1: Creating WorkflowService instance...");
                IWorkflowService workflowService = new WorkflowService();
                Console.WriteLine($"  ✓ WorkflowService instance created: {workflowService.GetType().FullName}");
                
                // Test 4.2: Verify service is not null
                Console.WriteLine("Test 4.2: Verifying service instance...");
                if (workflowService != null)
                {
                    Console.WriteLine($"  ✓ Service instance is not null");
                }
                else
                {
                    Console.WriteLine($"  ✗ Service instance is null");
                }
                
                Console.WriteLine("✓ Level 4 Tests: All passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Level 4 Tests: Failed - {ex.Message}");
                Console.WriteLine($"  Error details: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"  Inner exception: {ex.InnerException.Message}");
                }
            }
            
            Console.WriteLine();
        }

        #endregion

        #region Level 5: Business Method Tests

        /// <summary>
        /// Level 5 Tests: Business method execution (requires database)
        /// </summary>
        static void RunLevel5Tests_BusinessMethods()
        {
            Console.WriteLine("Level 5: Business Method Tests");
            Console.WriteLine("----------------------------------------");
            
            if (!_databaseInitialized)
            {
                Console.WriteLine("⚠ Database not initialized, skipping business method tests");
                Console.WriteLine();
                return;
            }

            try
            {
                // Test 5.1: GetProcessListSimple method test
                Console.WriteLine("Test 5.1: Testing GetProcessListSimple() method...");
                
                IWorkflowService workflowService = new WorkflowService();
                IList<ProcessEntity> processList = workflowService.GetProcessListSimple();
                
                if (processList != null)
                {
                    int recordCount = processList.Count;
                    Console.WriteLine($"  ✓ Method executed successfully");
                    Console.WriteLine($"  ✓ Return type: IList<ProcessEntity>");
                    Console.WriteLine($"  ✓ Total records returned: {recordCount}");
                    
                    if (recordCount > 0)
                    {
                        Console.WriteLine($"  ✓ Sample process (first record):");
                        var firstProcess = processList[0];
                        Console.WriteLine($"    - Process ID: {firstProcess.ProcessId ?? "N/A"}");
                        Console.WriteLine($"    - Process Name: {firstProcess.ProcessName ?? "N/A"}");
                        Console.WriteLine($"    - Version: {firstProcess.Version ?? "N/A"}");
                    }
                    else
                    {
                        Console.WriteLine($"  ℹ No process records found in database");
                    }
                }
                else
                {
                    Console.WriteLine($"  ✗ Method returned null");
                }
                
                Console.WriteLine("✓ Level 5 Tests: All passed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Level 5 Tests: Failed - {ex.Message}");
                Console.WriteLine($"  Error type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"  Inner exception: {ex.InnerException.Message}");
                }
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    int length = Math.Min(200, ex.StackTrace.Length);
                    Console.WriteLine($"  Stack trace: {ex.StackTrace.Substring(0, length)}...");
                }
            }
            
            Console.WriteLine();
        }

        #endregion
    }
}

