<%@ Page Title="" Language="C#" MasterPageFile="~/Registry.Master" AutoEventWireup="true" CodeBehind="courses.aspx.cs" Inherits="CourseCancellationRegistry.courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2 id="ContentTitle" runat="server">Welcome to the Course Cancellation Registry</h2>

    <asp:Label ID="emailLabel" runat="server" Text="Your email: " 
        EnableViewState="False"></asp:Label>
    <br /><br />
    The Course Cancellation Registry will email you class cancellations for the courses that you select.
    <br /><br />
    Add a course to your selected classes:
    <br />
    <asp:DropDownList ID="CoursesDropDownList" runat="server" Width="400px">
    </asp:DropDownList>
    <asp:Button ID="AddButton" runat="server" Text="Add" 
        onclick="AddButton_Click" Width="75px" />
    <br />
    <br />
    Your selected classes:
    <br />
    <asp:DropDownList ID="StudentCoursesDropDownList" runat="server" Width="400px">
    </asp:DropDownList>
    <asp:Button ID="RemoveButton" runat="server" Text="Remove" 
        onclick="RemoveButton_Click" Width="75px" />
    <br />
    <br />
    <asp:Button ID="ExitButton" runat="server" Text="Exit" Width="75px" 
        onclick="ExitButton_Click" />
</asp:Content>
