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
    public class CreateContactAssociateOwner
    {
        #region Class Level Members
        /// <summary>
        /// Stores the organization service proxy.
        /// </summary>
        private OrganizationServiceProxy _serviceProxy;
        private IOrganizationService _service;

        // Define the IDs needed for this sample.
        //private Guid _accountId;
        private Guid _contactId;
        private Guid _accountId;
        private string accountName;
        private string foundAccountName;
        private EntityReference foundOwnerId;
        private OrganizationServiceContext _orgContext;
        private string fName;
        private string lName;

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
            while (true)
            {
                Console.Write("Find Account Name: ");
                accountName = Console.ReadLine();
                if (accountName != string.Empty)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Account Name can't be empty!");
                }
            }

            while (true)
            {
                Console.Write("Contacts First Name: ");
                fName = Console.ReadLine();
                if (fName != string.Empty)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Contact Name can't be empty!");
                }
            }

            while (true)
            {

                Console.Write("Contacts Last Name: ");
                lName = Console.ReadLine();
                if (lName != string.Empty)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Contact Name can't be empty!");
                }
            }


            try
            {
                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                using (_serviceProxy = new OrganizationServiceProxy(serverConfig.OrganizationUri, serverConfig.HomeRealmUri, serverConfig.Credentials, serverConfig.DeviceCredentials))
                {
                    _service = (IOrganizationService)_serviceProxy;
                    _orgContext = new OrganizationServiceContext(_service);

                    var account = FindAccount(accountName);
                    _accountId = account.Id;
                    if (_accountId == null)
                    {
                        Console.WriteLine("Account was not found!"); ;
                    }
                    else
                    {
                        EntityReferenceCollection myEntityReferenceCollection = CreateAContact(_accountId, account);

                        //EntityReferenceCollection myEntityReferenceCollection = CreateContact(_accountId);

                        //AssociateConctactWithAccount(myEntityReferenceCollection);
                    }
                }

            }

            // Catch any service fault exceptions that Microsoft Dynamics CRM throws.
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                // You can handle an exception here or pass it back to the calling method.
                throw;
            }
        }

        public Entity FindAccount(string accountName)
        {
            // Instantiate an account object.
            Entity account = new Entity("account");
            account["name"] = accountName;

            AttributeCollection col = account.Attributes;

            foreach (var item in col)
            {
                Console.WriteLine(item);
            }

            //Create a query expression specifying the link entity alias and the columns of the link entity that you want to return
            QueryExpression qe = new QueryExpression();
            qe.EntityName = "account";
            qe.ColumnSet = new ColumnSet();
            qe.ColumnSet.Columns.Add("name");
            qe.ColumnSet.Columns.Add("ownerid");
            qe.ColumnSet.Columns.Add("accountid");

            //qe.LinkEntities.Add(new LinkEntity("account", "contact", "primarycontactid", "contactid", JoinOperator.Inner));
            //qe.LinkEntities[0].Columns.AddColumns("firstname", "lastname");
            //qe.LinkEntities[0].EntityAlias = "primarycontact";

            EntityCollection ec = _service.RetrieveMultiple(qe);

            Console.WriteLine("Retrieved {0} entities", ec.Entities.Count);

            foreach (Entity act in ec.Entities)
            {
                //col = act.Attributes;

                //foreach (var item in col)
                //{
                //    Console.WriteLine("item: " + item);
                //}

                foundAccountName = (string)act["name"];
                foundOwnerId = (EntityReference)act["ownerid"];
                //Console.WriteLine("account name:" + act["name"]);
                //Console.WriteLine("owner:" + foundOwnerId.LogicalName);
                Console.WriteLine("Search account: {0}, Found account: {1}", accountName, foundAccountName);
                if (accountName == foundAccountName)
                {
                    Console.WriteLine("accountid:" + act["accountid"]);
                    Console.WriteLine("account name:" + act["name"]);
                    Console.WriteLine("owner:" + foundOwnerId.LogicalName);
                    Console.WriteLine("Account found {0}", foundAccountName);
                    account = act;
                    break;
                }
            }
            return account;
        }

        public EntityReferenceCollection CreateAContact(Guid _accountId, Entity account)
        {
            Contact contact = new Contact()
            {
                FirstName = fName,
                LastName = lName,
                Address1_Line1 = "23 Market St.",
                Address1_City = "Sammamish",
                Address1_StateOrProvince = "MT",
                Address1_PostalCode = "99999",
                Telephone1 = "12345678",
                EMailAddress1 = fName + "." + lName + "@" + accountName + ".com",
                Id = Guid.NewGuid()
            };
            _contactId = contact.Id;
            _orgContext.AddObject(contact);

            Console.WriteLine("Service created with First Name {0} and Last Name {1}", contact.FirstName, contact.LastName);

            //Create a collection of the entity id(s) that will be associated to the contact.
            EntityReferenceCollection relatedEntities = new EntityReferenceCollection();
            relatedEntities.Add(new EntityReference("account", _accountId));

            _orgContext.Attach(account);
            //_orgContext.Attach(contact);
            _orgContext.AddLink(
                account,
                new Relationship("contact_customer_accounts"),
                contact);
            //SaveChangesHelper(contact, account);
            Console.WriteLine("and adding contact to account's contact list.");

            return relatedEntities;
        }

        public EntityReferenceCollection CreateContact(Guid _accountId)
        {
            // Instantiate a contact entity record and set its property values.
            Entity setupContact = new Entity("contact");
            setupContact["firstname"] = fName;
            setupContact["lastname"] = lName;


            _contactId = _service.Create(setupContact);
            Console.WriteLine("Created {0} {1}", setupContact["firstname"], setupContact["lastname"]);

            //Create a collection of the entity id(s) that will be associated to the contact.
            EntityReferenceCollection relatedEntities = new EntityReferenceCollection();
            relatedEntities.Add(new EntityReference("account", _accountId));

            return relatedEntities;
        }


        public void AssociateConctactWithAccount(EntityReferenceCollection relatedEntities)
        {
            // Create an object that defines the relationship between the contact and account.
            Relationship relationship = new Relationship("contact_as_primary_contact");
            //Relationship relationship = new Relationship("contact_as_responsible_contact");

            //Associate the contact with the account(s).
            _service.Associate("contact", _contactId, relationship, relatedEntities);

            Console.WriteLine("The entities have been associated.");
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

                CreateContactAssociateOwner app = new CreateContactAssociateOwner();
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
