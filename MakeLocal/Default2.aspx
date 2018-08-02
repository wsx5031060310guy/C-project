<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConflictDetection="CompareAllValues"
            ConnectionString="<%$ ConnectionStrings:connStr %>"
            DeleteCommand="DELETE FROM [tb_company] WHERE [id] = @original_id AND (([zip_no] = @original_zip_no) OR ([zip_no] IS NULL AND @original_zip_no IS NULL)) AND (([company_title] = @original_company_title) OR ([company_title] IS NULL AND @original_company_title IS NULL)) AND (([address] = @original_address) OR ([address] IS NULL AND @original_address IS NULL)) AND (([lat] = @original_lat) OR ([lat] IS NULL AND @original_lat IS NULL)) AND (([lng] = @original_lng) OR ([lng] IS NULL AND @original_lng IS NULL)) AND (([company_desc] = @original_company_desc) OR ([company_desc] IS NULL AND @original_company_desc IS NULL)) AND (([iconName] = @original_iconName) OR ([iconName] IS NULL AND @original_iconName IS NULL))"
            InsertCommand="INSERT INTO [tb_company] ([zip_no], [company_title], [address], [lat], [lng], [company_desc], [iconName]) VALUES (@zip_no, @company_title, @address, @lat, @lng, @company_desc, @iconName)"
            OldValuesParameterFormatString="original_{0}"
            oninserting="SqlDataSource1_Inserting"
            SelectCommand="SELECT * FROM [tb_company]"
            UpdateCommand="UPDATE [tb_company] SET [zip_no] = @zip_no, [company_title] = @company_title, [address] = @address, [lat] = @lat, [lng] = @lng, [company_desc] = @company_desc, [iconName] = @iconName WHERE [id] = @original_id AND (([zip_no] = @original_zip_no) OR ([zip_no] IS NULL AND @original_zip_no IS NULL)) AND (([company_title] = @original_company_title) OR ([company_title] IS NULL AND @original_company_title IS NULL)) AND (([address] = @original_address) OR ([address] IS NULL AND @original_address IS NULL)) AND (([lat] = @original_lat) OR ([lat] IS NULL AND @original_lat IS NULL)) AND (([lng] = @original_lng) OR ([lng] IS NULL AND @original_lng IS NULL)) AND (([company_desc] = @original_company_desc) OR ([company_desc] IS NULL AND @original_company_desc IS NULL)) AND (([iconName] = @original_iconName) OR ([iconName] IS NULL AND @original_iconName IS NULL))">
            <DeleteParameters>
                <asp:Parameter Name="original_id" Type="Int32" />
                <asp:Parameter Name="original_zip_no" Type="Int32" />
                <asp:Parameter Name="original_company_title" Type="String" />
                <asp:Parameter Name="original_address" Type="String" />
                <asp:Parameter Name="original_lat" Type="Double" />
                <asp:Parameter Name="original_lng" Type="Double" />
                <asp:Parameter Name="original_company_desc" Type="String" />
                <asp:Parameter Name="original_iconName" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="zip_no" Type="Int32" />
                <asp:Parameter Name="company_title" Type="String" />
                <asp:Parameter Name="address" Type="String" />
                <asp:Parameter Name="lat" Type="Double" />
                <asp:Parameter Name="lng" Type="Double" />
                <asp:Parameter Name="company_desc" Type="String" />
                <asp:Parameter Name="iconName" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="zip_no" Type="Int32" />
                <asp:Parameter Name="company_title" Type="String" />
                <asp:Parameter Name="address" Type="String" />
                <asp:Parameter Name="lat" Type="Double" />
                <asp:Parameter Name="lng" Type="Double" />
                <asp:Parameter Name="company_desc" Type="String" />
                <asp:Parameter Name="iconName" Type="String" />
                <asp:Parameter Name="original_id" Type="Int32" />
                <asp:Parameter Name="original_zip_no" Type="Int32" />
                <asp:Parameter Name="original_company_title" Type="String" />
                <asp:Parameter Name="original_address" Type="String" />
                <asp:Parameter Name="original_lat" Type="Double" />
                <asp:Parameter Name="original_lng" Type="Double" />
                <asp:Parameter Name="original_company_desc" Type="String" />
                <asp:Parameter Name="original_iconName" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <table style="width:100%;">
            <tr>
                <td>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                        ConnectionString="<%$ ConnectionStrings:connStr %>"
                        SelectCommand="SELECT * FROM [tb_company]"></asp:SqlDataSource>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" DataKeyNames="id" DataSourceID="SqlDataSource1"
                        ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="True"
                                        CommandName="Update" Text="更新" />
                                    &nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False"
                                        CommandName="Cancel" Text="取消" />
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="新增" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="Button1" runat="server" CausesValidation="False"
                                        CommandName="Edit" Text="編輯" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="編號" InsertVisible="False"
                                ReadOnly="True" SortExpression="id" />
                            <asp:BoundField DataField="company_title" HeaderText="名稱"
                                SortExpression="company_title" />
                            <asp:BoundField DataField="address" HeaderText="地址" SortExpression="address" />
                            <asp:BoundField DataField="lat" HeaderText="座標lat" SortExpression="lat" />
                            <asp:BoundField DataField="lng" HeaderText="座標lng" SortExpression="lng" />
                            <asp:BoundField DataField="company_desc" HeaderText="描述"
                                SortExpression="company_desc" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:FormView ID="FormView1" runat="server" CellPadding="4" DataKeyNames="id"
                        DataSourceID="SqlDataSource1" DefaultMode="Insert" ForeColor="#333333"
                        oniteminserted="FormView1_ItemInserted" Visible="False">
                        <EditItemTemplate>
                            id:
                            <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                            <br />
                            zip_no:
                            <asp:TextBox ID="zip_noTextBox" runat="server" Text='<%# Bind("zip_no") %>' />
                            <br />
                            company_title:
                            <asp:TextBox ID="company_titleTextBox" runat="server"
                                Text='<%# Bind("company_title") %>' />
                            <br />
                            address:
                            <asp:TextBox ID="addressTextBox" runat="server" Text='<%# Bind("address") %>' />
                            <br />
                            lat:
                            <asp:TextBox ID="latTextBox" runat="server" Text='<%# Bind("lat") %>' />
                            <br />
                            lng:
                            <asp:TextBox ID="lngTextBox" runat="server" Text='<%# Bind("lng") %>' />
                            <br />
                            company_desc:
                            <asp:TextBox ID="company_descTextBox" runat="server"
                                Text='<%# Bind("company_desc") %>' />
                            <br />
                            iconName:
                            <asp:TextBox ID="iconNameTextBox" runat="server"
                                Text='<%# Bind("iconName") %>' />
                            <br />
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True"
                                CommandName="Update" Text="更新" />
                            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server"
                                CausesValidation="False" CommandName="Cancel" Text="取消" />
                        </EditItemTemplate>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <InsertItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("zip_no") %>'></asp:Label>
                            <br />
                            名稱:<br />
                            <asp:TextBox ID="company_titleTextBox" runat="server"
                                Text='<%# Bind("company_title") %>' />
                            <br />
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red"
                                Text="地址或座標選一個寫就好!"></asp:Label>
                            <br />
                            <asp:Label ID="Label5" runat="server"
                                Text="---------------------------------------------"></asp:Label>
                            <br />
                            地址:<br />
                            <asp:TextBox ID="addressTextBox" runat="server" Height="30px"
                                Text='<%# Bind("address") %>' Width="214px" />
                            <br />
                            <asp:Label ID="Label4" runat="server"
                                Text="---------------------------------------------"></asp:Label>
                            <br />
                            座標lat:<br />
                            <asp:TextBox ID="latTextBox" runat="server" Text='<%# Bind("lat") %>' />
                            <br />
                            座標lng:<br />
                            <asp:TextBox ID="lngTextBox" runat="server" Text='<%# Bind("lng") %>' />
                            <br />
                            <asp:Label ID="Label6" runat="server"
                                Text="---------------------------------------------"></asp:Label>
                            <br />
                            描述:<br />
                            <asp:TextBox ID="company_descTextBox" runat="server" Height="23px"
                                Text='<%# Bind("company_desc") %>' Width="215px" />
                            <br />
                            &nbsp;<asp:Label ID="Label2" runat="server" Text='<%# Bind("iconName") %>'></asp:Label>
                            <br />
                            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True"
                                CommandName="Insert" Text="新增" />
                            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server"
                                CausesValidation="False" CommandName="Cancel" Text="取消" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            id:
                            <asp:Label ID="idLabel" runat="server" Text='<%# Eval("id") %>' />
                            <br />
                            zip_no:
                            <asp:Label ID="zip_noLabel" runat="server" Text='<%# Bind("zip_no") %>' />
                            <br />
                            company_title:
                            <asp:Label ID="company_titleLabel" runat="server"
                                Text='<%# Bind("company_title") %>' />
                            <br />
                            address:
                            <asp:Label ID="addressLabel" runat="server" Text='<%# Bind("address") %>' />
                            <br />
                            lat:
                            <asp:Label ID="latLabel" runat="server" Text='<%# Bind("lat") %>' />
                            <br />
                            lng:
                            <asp:Label ID="lngLabel" runat="server" Text='<%# Bind("lng") %>' />
                            <br />
                            company_desc:
                            <asp:Label ID="company_descLabel" runat="server"
                                Text='<%# Bind("company_desc") %>' />
                            <br />
                            iconName:
                            <asp:Label ID="iconNameLabel" runat="server" Text='<%# Bind("iconName") %>' />
                            <br />
                            <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False"
                                CommandName="Edit" Text="編輯" />
                            &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False"
                                CommandName="Delete" Text="刪除" />
                            &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False"
                                CommandName="New" Text="新增" />
                        </ItemTemplate>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    </asp:FormView>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
