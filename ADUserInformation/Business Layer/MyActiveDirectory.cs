using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.DirectoryServices;
//using System.DirectoryServices.Protocols;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace ADUserInformation.Business_Layer
{

    public class MyActiveDirectory
    {
        public string LDAPPath;

        public MyActiveDirectory()
        {
            LDAPPath = ConfigurationManager.AppSettings["LDAPPath"].ToString();
        }

        public DirectorySearcher CreateDirectorySearcher(string objectName)
        {
            try
            {
                //----------------------------------------------------Start
                //string hostOrDomainName = "lthed.com";

                ////string targetOu = "cn=builtin,dc=lthed,dc=com";
                //string targetOu = "cn=users,dc=lthed,dc=com";

                //// create a search filter to find all objects
                //string ldapSearchFilter = "(objectClass=*)";


                //// establish a connection to the directory
                //LdapConnection connection = new LdapConnection(hostOrDomainName);

                //System.Net.NetworkCredential credential = new System.Net.NetworkCredential("spsadmin", "SPS123", "LTHED");
                //connection.Credential = credential;


                //SearchRequest searchRequest = new SearchRequest
                //                    (targetOu,
                //                      ldapSearchFilter,
                //                      System.DirectoryServices.Protocols.SearchScope.Subtree,
                //                      null);

                //// cast the returned directory response as a SearchResponse object
                //SearchResponse searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

                //int myCount = searchResponse.Entries.Count;

                //// enumerate the entries in the search response
                //foreach (SearchResultEntry entry in searchResponse.Entries)
                //{
                //    //Console.WriteLine("{0}:{1}", searchResponse.Entries.IndexOf(entry), entry.DistinguishedName);
                //    string SearchName = entry.DistinguishedName;

                //}



                //----------------------------------------------------End

                
                
                

                DirectorySearcher mySearcher = null;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    DirectoryEntry entry = new DirectoryEntry(LDAPPath);
                    entry.Username = "spsadmin";
                    entry.Password = "SPS123";


                   // entry.Username = ConfigurationManager.AppSettings["CarrierUsername"].ToString();
                    //entry.Password = ConfigurationManager.AppSettings["CarrierPassword"].ToString();


                    mySearcher = new DirectorySearcher(entry);
                    //mySearcher
                    // We are searching on CN and DN. We also should be searching onn first name and last name.
                    //switch (objectCls)
                    //{
                    //    case dbEnum.objectClass.user:
                            mySearcher.Filter = "(&(objectClass=user)(|(cn=" + objectName + ")(sAMAccountName=" + objectName + ")))";
                    //        break;
                    //    case dbEnum.objectClass.group: mySearcher.Filter = "(&(objectClass=group)(|(cn=" + objectName + ")(dn=" + objectName + ")))";
                    //        break;
                    //    case dbEnum.objectClass.computer: mySearcher.Filter = "(&(objectClass=computer)(|(cn=" + objectName + ")(dn=" + objectName + ")))";
                    //        break;
                    //    case dbEnum.objectClass.userInGroup:
                    //        mySearcher.Filter = "(&(objectClass=user))";
                    //        break;
                    //}

                    mySearcher.SizeLimit = 0;
                    mySearcher.PageSize = 999;
                    mySearcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                    mySearcher.PropertiesToLoad.Add("samaccountname");
                    mySearcher.PropertiesToLoad.Add("mail");
                    mySearcher.PropertiesToLoad.Add("l");
                    mySearcher.PropertiesToLoad.Add("sn");
                    mySearcher.PropertiesToLoad.Add("department");
                    mySearcher.PropertiesToLoad.Add("givenname");// first name                    
                    mySearcher.PropertiesToLoad.Add("manager");
                    mySearcher.PropertiesToLoad.Add("dn");
                });
                return mySearcher;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GetProperty(SearchResult searchResult, string PropertyName)
        {
            try
            {
                if (searchResult.Properties.Contains(PropertyName))
                {                        
                    if (PropertyName == "manager")
	                {
                        string strMGRName = Convert.ToString(searchResult.Properties[PropertyName][0]).Trim();
                        string YourManager = strMGRName.Substring(strMGRName.IndexOf("=") + 1, strMGRName.IndexOf(",") -3);
                        //txtYourManager.Text = (result.Properties("manager")(0)).toString().Trim() 'YourManager
                        return Convert.ToString(YourManager);
	                }
                    else
                    {
                        return Convert.ToString(searchResult.Properties[PropertyName][0]);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public DataTable getUsersList(string username)
        {
            string[] name = username.Trim().Split(' ');
            DataTable _dtEmployees = new DataTable("Employees");
            _dtEmployees.Columns.Add(new DataColumn("username"));
            _dtEmployees.Columns.Add(new DataColumn("firstname"));
            _dtEmployees.Columns.Add(new DataColumn("lastname"));
            _dtEmployees.Columns.Add(new DataColumn("department"));
            _dtEmployees.Columns.Add(new DataColumn("mail"));
            System.Text.StringBuilder strbldr = new System.Text.StringBuilder();
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    DirectoryEntry myDE = new DirectoryEntry(LDAPPath);
                    myDE.AuthenticationType = AuthenticationTypes.Secure;
                    DirectorySearcher mySearcher = new DirectorySearcher(myDE);
                    mySearcher.PropertiesToLoad.Add("givenname");
                    mySearcher.PropertiesToLoad.Add("sn");
                    mySearcher.PropertiesToLoad.Add("mail");
                    mySearcher.PropertiesToLoad.Add("department");
                    string searchFilter = "";

                    if (username.Contains(" "))
                    {
                        searchFilter = "(&(objectClass=user)(givenname=" + name[0] + "*)(sn=" + name[name.Length - 1] + "*))";
                    }
                    else
                    {
                        searchFilter = "(&(objectClass=user)(|(givenname=" + username + "*)(sn=" + username + "*)))";
                    }
                    mySearcher.Filter = searchFilter;

                    SearchResultCollection myResultCollection = mySearcher.FindAll();

                    foreach (SearchResult myResult in myResultCollection)
                    {
                        if (myResult != null)
                        {
                            DataRow _dr = _dtEmployees.NewRow();

                            if ((myResult.Properties.Contains("givenname")) && myResult.Properties["givenname"][0] != null)
                            {
                                _dr["firstname"] = myResult.Properties["givenname"][0].ToString();
                                if ((myResult.Properties.Contains("sn")) && myResult.Properties["sn"][0] != null)
                                {
                                    _dr["lastname"] = myResult.Properties["sn"][0].ToString();
                                }
                                if ((myResult.Properties.Contains("department")) && myResult.Properties["department"][0] != null)
                                {
                                    _dr["department"] = myResult.Properties["department"][0].ToString();
                                }
                                if ((myResult.Properties.Contains("mail")) && myResult.Properties["mail"][0] != null)
                                {
                                    _dr["mail"] = myResult.Properties["mail"][0].ToString();
                                }
                                _dtEmployees.Rows.Add(_dr);
                            }

                        }

                    }
                    if (username.Contains(" ") && _dtEmployees.Rows.Count == 0)
                    {
                        searchFilter = "(&(objectClass=user)(givenname=" + name[name.Length - 1] + "*)(sn=" + name[0] + "*))";

                        mySearcher.Filter = searchFilter;

                        myResultCollection = mySearcher.FindAll();

                        foreach (SearchResult myResult in myResultCollection)
                        {
                            if (myResult != null)
                            {
                                DataRow _dr = _dtEmployees.NewRow();

                                if ((myResult.Properties.Contains("givenname")) && myResult.Properties["givenname"][0] != null)
                                {
                                    _dr["firstname"] = myResult.Properties["givenname"][0].ToString();
                                    if ((myResult.Properties.Contains("sn")) && myResult.Properties["sn"][0] != null)
                                    {
                                        _dr["lastname"] = myResult.Properties["sn"][0].ToString();
                                    }
                                    if ((myResult.Properties.Contains("department")) && myResult.Properties["department"][0] != null)
                                    {
                                        _dr["department"] = myResult.Properties["department"][0].ToString();
                                    }
                                    if ((myResult.Properties.Contains("mail")) && myResult.Properties["mail"][0] != null)
                                    {
                                        _dr["mail"] = myResult.Properties["mail"][0].ToString();
                                    }
                                    _dtEmployees.Rows.Add(_dr);
                                }

                            }

                        }
                    }
                });
            }

            catch (Exception ex)
            {

                DataRow dr = _dtEmployees.NewRow();
                dr["username"] = "<br> " + ex.Message + "<br>";
                _dtEmployees.Rows.Add(dr);
            }

            return _dtEmployees;
        }

    }
}
