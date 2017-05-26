// =====================================================================
//
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
//
// =====================================================================
//<snippetCRUDOperationsDE>
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

// These namespaces are found in the Microsoft.Xrm.Sdk.dll assembly
// found in the SDK\bin folder.
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Crm.Sdk.Samples.Model;


namespace Microsoft.Crm.Sdk.Samples
{
    /// <summary>
    /// Demonstrates how to do basic entity operations like create, retrieve,
    /// update, and delete.
    /// If you want to run this sample repeatedly, you have the option to 
    /// delete all the records created at the end of execution.
    /// </summary>
    public class CreateContactAssociateAccount 
    {
        #region Class Level Members
        /// <summary>
        /// Stores the organization service proxy.
        /// </summary>
        private OrganizationServiceProxy _serviceProxy;
        private IOrganizationService _service;

        // Define the IDs needed for this sample.
        private Guid _accountId;

        #endregion Class Level Members

        #region How To Sample Code
        /// <summary>
        /// Create and configure the organization service proxy.
        /// Create an account record
        /// Retrieve the account record
        /// Update the account record
        /// Optionally delete any entity records that were created for this sample.
        /// </summary>
        /// <param name="serverConfig">Contains server connection information.</param>
        /// <param name="promptForDelete">When True, the user will be prompted to delete
        /// all created entities.</param>
        public void Run(ServerConnection.Configuration serverConfig, bool promptForDelete)
        {
            AccountModel accountModel  = new AccountModel();

            while (true)
            {
                Console.Write("Account Name: ");
                accountModel.AccountName = Console.ReadLine();
                if (accountModel.AccountName != string.Empty)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Account Name can't be empty!");
                }
            }

            //Console.Write("Adress Row 1: ");
            //accountModel.AdressRow1 = Console.ReadLine();
            //Console.Write("Adress Row 2: ");
            //accountModel.AdressRow2 = Console.ReadLine();
            //while (true)
            //{
            //    Console.Write("Expected Revenue: ");
            //    string tempRevenue = Console.ReadLine();
            //    int tempIntRevenue = 0;

            //    bool result = Int32.TryParse(tempRevenue, out tempIntRevenue);
            //    if (result)
            //    {
            //        accountModel.Revenue = tempIntRevenue;
            //        break;
            //    }
            //    else
            //    {
            //        Console.WriteLine("Attempted conversion of '{0}' failed.",
            //                           tempRevenue == null ? "<null>" : tempRevenue);
            //    }

            //}

            //char tempAccountName = ' ';
            //Console.Write("Credit limit? (y/n) [y]: ");
            //try
            //{
            //    tempAccountName = Console.ReadLine()[0];
            //    if (tempAccountName == 'n')
            //    {
            //        accountModel.CreditOnHold = false;
            //        Console.WriteLine("Credit was set to no");
            //    }
            //    else
            //    {
            //        accountModel.CreditOnHold = true;
            //        Console.WriteLine("Credit was set to yes");
            //    }
            //}
            //catch (Exception)
            //{
            //    accountModel.CreditOnHold = true;
            //    Console.WriteLine("Credit was set to yes");
            //}

            try
            {

                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = new OrganizationServiceProxy(serverConfig.OrganizationUri, serverConfig.HomeRealmUri, serverConfig.Credentials, serverConfig.DeviceCredentials))
                {
                    _service = (IOrganizationService)_serviceProxy;

                    //<snippetCRUDOperationsDE1>
                    // Instaniate an account object.
                    Entity account = new Entity("account");

                    // Set the required attributes. For account, only the name is required. 
                    // See the Entity Metadata topic in the SDK documentatio to determine 
                    // which attributes must be set for each entity.
                    account["name"] = accountModel.AccountName;

                    // Create an account record named Fourth Coffee.
                    //_accountId = _service.Create(account);

                    //Console.Write("{0} {1} created, ", account.LogicalName, account.Attributes["name"]);

                    // Create a column set to define which attributes should be retrieved.
                    ColumnSet attributes = new ColumnSet(new string[] { "name", "ownerid", "address1_postalcode", "address2_postalcode", "revenue", "creditonhold" });

                    // Retrieve the account and its name and ownerid attributes.
                    account = _service.Retrieve(account.LogicalName, _accountId, attributes);
                    Console.Write("retrieved, ");

                    Console.WriteLine(account["name"]);
                    Console.WriteLine(account["ownerid"]);

                    //// Update the postal code attribute.
                    //if (accountModel.AdressRow1 != string.Empty)
                    //{
                    //    account["address1_postalcode"] = accountModel.AdressRow1;
                    //}
                    //else
                    //{
                    //    account["address1_postalcode"] = null;
                    //}

                    //// The address 2 postal code was set accidentally, so set it to null.
                    //if (accountModel.AdressRow2 != string.Empty)
                    //{
                    //    account["addres2_postalcode"] = accountModel.AdressRow2;
                    //}
                    //else
                    //{
                    //    account["address2_postalcode"] = null;
                    //}

                    //// Shows use of Money.
                    //account["revenue"] = new Money(accountModel.Revenue);

                    //// Shows use of boolean.
                    //account["creditonhold"] = accountModel.CreditOnHold;

                    //// Update the account.
                    //_service.Update(account);
                    //Console.WriteLine("and updated.");

                    //// Delete the account.
                    //bool deleteRecords = true;

                    //if (promptForDelete)
                    //{
                    //    Console.WriteLine("\nDo you want these entity records deleted? (y/n) [y]: ");
                    //    String answer = Console.ReadLine();

                    //    deleteRecords = (answer.StartsWith("y") || answer.StartsWith("Y") || answer == String.Empty);
                    //}

                    //if (deleteRecords)
                    //{
                    //    _service.Delete("account", _accountId);

                    //    Console.WriteLine("Entity record(s) have been deleted.");
                    //}

                    //</snippetCRUDOperationsDE1>
                }
            }

            // Catch any service fault exceptions that Microsoft Dynamics CRM throws.
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                // You can handle an exception here or pass it back to the calling method.
                throw;
            }
        }

        #endregion How To Sample Code

        #region Main
        /// <summary>
        /// Standard Main() method used by most SDK samples.
        /// </summary>
        /// <param name="args"></param>
        static public void Main(string[] args)
        {
            try
            {
                // Obtain the target organization's Web address and client logon 
                // credentials from the user.
                ServerConnection serverConnect = new ServerConnection();
                ServerConnection.Configuration config = serverConnect.GetServerConfiguration();

                CRUDOperationsDE app = new CRUDOperationsDE();
                app.Run(config, true);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Timestamp: {0}", ex.Detail.Timestamp);
                Console.WriteLine("Code: {0}", ex.Detail.ErrorCode);
                Console.WriteLine("Message: {0}", ex.Detail.Message);
                Console.WriteLine("Plugin Trace: {0}", ex.Detail.TraceText);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
            }
            catch (System.TimeoutException ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine("Message: {0}", ex.Message);
                Console.WriteLine("Stack Trace: {0}", ex.StackTrace);
                Console.WriteLine("Inner Fault: {0}",
                    null == ex.InnerException.Message ? "No Inner Fault" : ex.InnerException.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("The application terminated with an error.");
                Console.WriteLine(ex.Message);

                // Display the details of the inner exception.
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);

                    FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> fe = ex.InnerException
                        as FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>;
                    if (fe != null)
                    {
                        Console.WriteLine("Timestamp: {0}", fe.Detail.Timestamp);
                        Console.WriteLine("Code: {0}", fe.Detail.ErrorCode);
                        Console.WriteLine("Message: {0}", fe.Detail.Message);
                        Console.WriteLine("Plugin Trace: {0}", fe.Detail.TraceText);
                        Console.WriteLine("Inner Fault: {0}",
                            null == fe.Detail.InnerFault ? "No Inner Fault" : "Has Inner Fault");
                    }
                }
            }
            // Additional exceptions to catch: SecurityTokenValidationException, ExpiredSecurityTokenException,
            // SecurityAccessDeniedException, MessageSecurityException, and SecurityNegotiationException.

            finally
            {

                Console.WriteLine("Press <Enter> to exit.");
                Console.ReadLine();
            }

        }
        #endregion Main
    }
}
//</snippetCRUDOperationsDE>
