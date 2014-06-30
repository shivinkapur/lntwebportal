<%@ Assembly Name="ADUserInformation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d0393a1d8f31c01d" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpUserInfoUserControl.ascx.cs"
    Inherits="ADUserInformation.wpUserInfo.wpUserInfoUserControl" %>
<style type="text/css">
    .ddlWidth
    {
        width: 280px;
    }
    .col1
    {
        width: 150px;
        height: 32px;
    }
    .col2
    {
        width: 200px;
        height: 32px;
    }
    
    .rowStyle
    {
        border-top-style: solid;
        border-top-color: Gray;
        border-top-width: thin;
    }
</style>
<asp:UpdatePanel ID="upUserInfo" runat="server">
    <ContentTemplate>
    <asp:Label ID="lbldisp" runat="server" ForeColor="Red"></asp:Label>
        <table width="100%">
            <tr class="rowStyle">
                <td class="col1">
                    Name
                </td>
                <td class="col2">
                <asp:Label ID="lblName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="rowStyle">
                <td class="col1">
                    Department Code
                </td>
                <td class="col2">
                    <asp:Label ID="lblDeptCode" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="rowStyle">
                <td class="col1">
                    Location
                </td>
                <td class="col2">
                    <asp:Label ID="lblLocation" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="display: none;">
                <td class="col1">
                    IS ID
                </td>
                <td class="col2">
                    <asp:Label ID="lblIS" runat="server"> </asp:Label>
                </td>
            </tr>
            <tr class="rowStyle">
                <td class="col1">
                    IS Name:
                </td>
                <td class="col2">
                    <asp:Label ID="lblISName" runat="server"> </asp:Label>
                </td>
            </tr>
            <tr class="rowStyle">
                <td class="col1">
                    SBU:
                </td>
                <td class="col2">
                    <asp:Label ID="lblSbu" runat="server"> </asp:Label>
                </td>
            </tr>
           <%-- <tr class="rowStyle">
                <td class="col1">
                    Enter your SBU:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlSbu" runat="server" CssClass="ddlWidth">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <tr class="rowStyle">
                <td class="col1">
                    Function Name:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlFunctionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFunctionList_SelectedIndexChanged"
                        CssClass="ddlWidth">
                    </asp:DropDownList>
                </td>
            </tr>
            <%--<tr class="rowStyle">
                <td class="col1">
                    Module Name:
                </td>
                <td class="col2">
                    <asp:DropDownList ID="ddlModuleList" runat="server" CssClass="ddlWidth">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <tr class="rowStyle">
                <td class="col1">
                    Module Name:
                </td>
                <td class="col2">
                    <asp:CheckBoxList ID="lstModuleList" runat="server" SelectionMode="Multiple" >
                    </asp:CheckBoxList> 
                </td>
            </tr>
            <tr class="rowStyle">
                <td class="col1">
                    &nbsp;
                </td>
                <td class="col2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" Width="100px" />
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click"
                        Width="100px" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Label ID="lblmsg" Font-Bold="true" runat="server" ForeColor="Brown"> </asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<input type="hidden" id="hdISPsno" runat="server" />
<input type="hidden" id="hdISEmail" runat="server" />
<input type="hidden" id="hdLoginName" runat="server" />

