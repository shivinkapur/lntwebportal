<%@ Assembly Name="ADUserInformation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d0393a1d8f31c01d" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wpApprovalUserControl.ascx.cs"
    Inherits="ADUserInformation.wpApproval.wpApprovalUserControl" %>

<%--<script type="text/javascript">

    function SelectAllCheckboxes(spanChk) {

        // Added as ASPX uses SPAN for checkbox
        var oItem = spanChk.children;
        var theBox = (spanChk.type == "checkbox") ?
        spanChk : spanChk.children.item[0];
        xState = theBox.checked;
        elm = theBox.form.elements;

        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
                //elm[i].click();
                if (elm[i].checked != xState)
                    elm[i].click();
                //elm[i].checked=xState;
            }
    }
</script>--%>
<style type="text/css">
    .visi
    {
       display: none;
    }
</style>

<asp:GridView ID="grdAppPage" runat="server" AllowPaging="True" AllowSorting="false"
    AutoGenerateColumns="False" Width="400px" CellPadding="4" DataKeyNames="ID"
    EnableModelValidation="True" ForeColor="#333333" GridLines="None" EmptyDataText=" There are no more items to show. Please click on done to proceed. " > 

    <Columns>
<asp:TemplateField HeaderText="CheckAll">
<HeaderStyle HorizontalAlign="Left" />
<HeaderTemplate>
<asp:CheckBox ID="chkSelectAll" runat="server" 
              AutoPostBack="true" 
              OnCheckedChanged="chkSelectAll_CheckedChanged"/>
</HeaderTemplate>
<ItemStyle HorizontalAlign="Left" />
<ItemTemplate>
<asp:CheckBox ID="chkSelect" runat="server" />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="ID" HeaderText="ID" Visible="false" ControlStyle-CssClass="visi" 
                SortExpression="ID" HeaderStyle-HorizontalAlign="Left" />
<asp:BoundField DataField="apNames" HeaderText="Requestee Names" 
                HeaderStyle-HorizontalAlign="Left" />

<asp:BoundField DataField="apModule" HeaderText="Module Name" 
                HeaderStyle-HorizontalAlign="Left" />
</Columns>
    <EditRowStyle BackColor="#2461BF" />
<FooterStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
    <RowStyle BackColor="White" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
</asp:GridView>
<br />
<br />
<asp:Label ID="lblvalid" ForeColor="Red" runat="server" />
<br />
<br />
<asp:Button ID="btnApprove" runat="server" Text="Approve" 
    onclick="btnApprove_Click" Width="100px" />

<asp:Button ID="btnReject" runat="server" Text="Reject" 
    onclick="btnReject_Click" Width="100px" />

<asp:Button ID="btnDone" runat="server" Text="Done" 
    onclick="btnDone_Click" Width="100px" />




<%--<asp:GridView ID="grdAppPage" runat="server" AllowPaging="True" AllowSorting="True"
    AutoGenerateColumns="False" DataKeyNames="Status" Width="366px" CellPadding="4"
    ForeColor="#333333" GridLines="None">
    
    <Columns>
        <asp:CommandField ShowSelectButton="True" />
        <asp:BoundField DataField="Status" HeaderText="Approve/Reject" InsertVisible="False"
            ReadOnly="True" SortExpression="Status" />
        <asp:TemplateField HeaderText="Select">
            <HeaderTemplate>
                <input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                    type="checkbox" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="ichkSelect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />

</asp:GridView>--%>

<%--<asp:GridView ID="GridView1" runat="server" 
              DataSourceID="SqlDataSource1" 
              AutoGenerateColumns="false" 
              CellPadding="2" ForeColor="#333333" 
              GridLines="Both" 
              DataKeyNames="ID" 
              OnRowDataBound="GridView1_RowDataBound">
<Columns>
<asp:TemplateField HeaderText="CheckAll">
<HeaderTemplate>
<asp:CheckBox ID="chkSelectAll" runat="server" 
              AutoPostBack="true" 
              OnCheckedChanged="chkSelectAll_CheckedChanged"/>
</HeaderTemplate>
<ItemTemplate>
<asp:CheckBox ID="chkSelect" runat="server" 
              AutoPostBack="true" 
              OnCheckedChanged="chkSelect_CheckedChanged"/>
</ItemTemplate>
</asp:TemplateField>

<asp:BoundField DataField="ID" HeaderText="ID" 
                SortExpression="ID"/>
<asp:TemplateField HeaderText="Name" SortExpression="Name">
<ItemTemplate>
<asp:TextBox ID="txtName" runat="server" 
             Text='<%# Bind("Name") %>' ForeColor="Blue" 
             BorderStyle="none" BorderWidth="0px" 
             ReadOnly="true" >
</asp:TextBox>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Location" SortExpression
="Location">
<ItemTemplate>
<asp:TextBox ID="txtLocation" runat="server" 
             Text='<%# Bind("Location") %>' 
             ForeColor="Blue" BorderStyle="none" 
             ReadOnly="true">
</asp:TextBox>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
SelectCommand="SELECT [ID], [Name], [Location] FROM [Details]" 
DeleteCommand="DELETE FROM Details WHERE (ID = @ID)" 
UpdateCommand="UPDATE [Details] SET [Name] = @Name, 
               [Location] = @Location WHERE [ID] = @ID">
<DeleteParameters>
<asp:Parameter Name="ID" />
</DeleteParameters>

<UpdateParameters>
<asp:Parameter Name="Name" />
<asp:Parameter Name="Location" />
<asp:Parameter Name="ID" />
</UpdateParameters>
</asp:SqlDataSource>

<asp:Button ID="btnApprove" runat="server" 
            OnClick="btnApprove_Click" Text="Approve" />

<asp:Button ID="btnReject" runat="server" 
            OnClick="btnReject_Click" 
            Text="Reject" />

--%>

<%--<asp:GridView ID="grdAppPage" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="True"
    BackColor="White" BorderColor="#336699" BorderStyle="Solid" BorderWidth="1px"   
    CellPadding="0" CellSpacing="0" DataKeyNames="Status" Font-Size="10" onrowdatabound="grdAppPage_RowDataBound"
    Font-Names="Arial" GridLines="Vertical" Width="40%">

    <Columns>
        <asp:BoundField DataField="fgyfg" HeaderText="" />
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>--%>

<input type="hidden" id="hdAppLoginName" runat="server" />
