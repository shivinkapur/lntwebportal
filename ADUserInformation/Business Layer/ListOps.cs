using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Data;
using ADUserInformation.Business_Layer;
using System.Configuration;
using System.ComponentModel;
using System.Data.OleDb;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint.WebControls;

namespace ADUserInformation.Business_Layer
{
    public class ListOps
    {
            public bool AddInRequestFormLst(string pSbu, string pDeptCode,
            string pLocation, string pISPsno, string pModuleName, string pStatus)
        {

            bool blnResult = false;
            string strUrl = Convert.ToString(ConfigurationManager.AppSettings["SiteUrl"]);
            using (SPSite objSite = new SPSite(strUrl))
            {
                using (SPWeb objWeb = objSite.OpenWeb())
                {


                    SPList list = objWeb.Lists["Requestee List"];
                    SPUser objUser = objWeb.EnsureUser(pISPsno);
                    string strPeople = Convert.ToString(objUser.ID) + ";#" + Convert.ToString(objUser.LoginName) + ";#";
                    //string strPeople = Convert.ToString(objUser.LoginName);
                    // SPList listM = web.Lists["Module Master"];

                    //var lstRequest = (from SPListItem lstR in list.Items
                    //                  select lstR).ToList();
                    //string Module = string.Empty;
                    //foreach (var item in lstRequest)
                    //{
                    //    Module = Convert.ToString(item["Module Name"]);

                    //}

                    //SPList lstModuleMaster = objWeb.Lists["Module Master"];
                    SPListItem _splistitem = list.Items.Add();
                    
                    //  SPFieldLookupValueCollection spfModuleName = new SPFieldLookupValueCollection();

                    //foreach (SPListItem moduleName in listM.Items)
                    //{

                    //    spfModuleName.Add(new SPFieldLookupValue(Convert.ToInt32(moduleName.ID), pModuleName));

                    //}



                    _splistitem["IS"] = strPeople;
                    //_splistitem["ISName"] = pISName;
                    _splistitem["ModuleName"] = new SPFieldLookupValue(pModuleName); 
                    _splistitem["SBU"] = pSbu;
                    _splistitem["DepartmentCode"] = pDeptCode;
                    _splistitem["Location"] = pLocation;                    
                     _splistitem["Status"] = pStatus;

                    //int intSelectedId = getItemId(pModuleName, lstModuleMaster);
                    //SPFieldLookupValue spv;
                    //if (intSelectedId > 0)
                    //{
                    //    spv = new SPFieldLookupValue(intSelectedId, pModuleName);
                    //    _splistitem["Module Name"] = spv;
                    //}

                    _splistitem.Update();
                    objWeb.Update();

                    blnResult = true;

                        //_spweb.AllowUnsafeUpdates = false;
                        //SPListItem _splistitem = _splist.Items.Add();
                }
                
            }
            return blnResult;
        }

            public void Approval(int pintID)
            {
                string strUrl = Convert.ToString(ConfigurationManager.AppSettings["SiteUrl"]);

                using (SPSite objSite = new SPSite(strUrl))
                {
                    using (SPWeb objWeb = objSite.OpenWeb())
                    {


                        SPList list = objWeb.Lists["Requestee List"];


                        SPListItem selectedEmps = (from SPListItem item in list.Items
                                                   where Convert.ToInt32(item["ID"]) == pintID
                                                   select item).FirstOrDefault();

                        selectedEmps["Status"] = "Approved";
                        selectedEmps.Update();
                        objWeb.Update();

                    }
                }

            }

            public void Rejection(int pintID)
            {
                string strUrl = Convert.ToString(ConfigurationManager.AppSettings["SiteUrl"]);

                using (SPSite objSite = new SPSite(strUrl))
                {
                    using (SPWeb objWeb = objSite.OpenWeb())
                    {


                        SPList list = objWeb.Lists["Requestee List"];


                        SPListItem selectedEmps = (from SPListItem item in list.Items
                                                   where Convert.ToInt32(item["ID"]) == pintID
                                                   select item).FirstOrDefault();

                        selectedEmps["Status"] = "Rejected";
                        selectedEmps.Update();
                        objWeb.Update();

                    }
                }

            }



        #region Old Code

        // public bool AddInRequestFormLst(string pEmpName, string pLoginName, string pSbu, string pDeptCode,
        //    string pLocation, string pModuleName, string pISPsno, string pISName, string pISEmail)
        //{

        //    bool blnResult = false;
        //    using (SPWeb web = SPContext.Current.Web)
        //    {

        //        SPList list = web.Lists["Request Form"];

        //        var lstRequest = (from SPListItem lstR in list.Items
        //                          select lstR).ToList();

        //        foreach (var item in lstRequest)
        //        {
        //            string Module = Convert.ToString(item["Module Name"]);

        //        }

        //        SPList lstModuleMaster = web.Lists["Module Master"];

        //        SPListItem _splistitem = list.Items.Add();

        //        SPFieldLookup test = new SPFieldLookup(;
        //        _splistitem["IS Email"] = pISEmail;
        //        _splistitem["IS Name"] = pISName;
        //        _splistitem["Module Name"] = GetLookFieldIDS(pModuleName, lstModuleMaster); //new SPFieldLookupValue("1;#Company Structure") ;
        //        _splistitem["Names"] = pEmpName;
        //        _splistitem["SBU"] = pSbu;
        //        _splistitem["Department Code"] = pDeptCode;
        //        _splistitem["Location"] = pLocation;
        //        _splistitem["Employee PSNO"] = pLoginName;
        //        _splistitem["IS PSNO"] = pISPsno;

        //        _splistitem.Update();
        //        web.Update();

        //        blnResult = true;

        //    }
        //    return blnResult;
        //}

        //        public static int getItemId(string strValue, SPList spList)
        //        {

        //            SPQuery query = new SPQuery();
        //            query.Query = @"<Where><Eq><FieldRef Name='TOPICS' />
        //                  <Value Type='TEXT'>" + strValue + "</Value></Eq></Where>";
        //              if (spList.GetItems(query) != null)
        //                    {
        //                        SPListItem result = spList.GetItems(query)[0];
        //                        return result.ID;
        //                     }
        //              else
        //                    return 0;
        //        }


        //public static SPFieldLookupValueCollection GetLookFieldIDS(string plookupValues, SPList lookupSourceList)
        //{
        //    SPFieldLookupValueCollection lookupIds = new SPFieldLookupValueCollection();
        //    string[] lookups = plookupValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (string lookupValue in lookups)
        //    {
        //        SPQuery query = new Microsoft.SharePoint.SPQuery();
        //        query.Query = String.Format("<Where><Eq><FieldRef Name='TOPICS'/><Value Type='Text'>{0}</Value></Eq></Where>", lookupValue);
        //        SPListItemCollection listItems = lookupSourceList.GetItems(query);
        //        foreach (Microsoft.SharePoint.SPListItem item in listItems)
        //        {
        //            SPFieldLookupValue value = new SPFieldLookupValue(Convert.ToInt32(item.ID), plookupValues);
        //            lookupIds.Add(value);
        //            break;
        //        }
        //    }
        //    return lookupIds;
        //}


        //private SPListItem AddRequestToList(List<SPUser> countryReps)
        //{

        //    SPList requests = SPContext.Current.Web.Lists["Requests"];

        //    SPListItem newRequestItem = requests.Items.Add();

        //    SPFieldUserValueCollection countryRepsToAdd = new SPFieldUserValueCollection();

        //    foreach (SPUser countryRep in countryReps)
        //    {

        //        countryRepsToAdd.Add(

        //        new SPFieldUserValue(

        //        SPContext.Current.Web, countryRep.ID, countryRep.Name));

        //    }



        //    newRequestItem["Title"] = txtSubject.Text;

        //    newRequestItem["Request Subject"] = txtSubject.Text;

        //    newRequestItem["Request Type"] = drpLstRequestType.SelectedItem.Text;

        //    newRequestItem["Due Date"] = dtDueDate.SelectedDate;

        //    newRequestItem["Country Representative"] = countryRepsToAdd;

        //    newRequestItem.Update();

        //    requests.Update();

        //    return newRequestItem;

        //}

        #endregion
    }
}
