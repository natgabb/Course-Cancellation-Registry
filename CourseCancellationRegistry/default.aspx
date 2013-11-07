<%@ Page Title="" Language="C#" MasterPageFile="~/Registry.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CourseCancellationRegistry._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2 id="ContentTitle">Sign in to the Course Cancellation Registry</h2>
Welcome Dawson students! The Course Cancellation Registry will email you class cancellations.
<br /><br />
Enter email: <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SubmitButton" runat="server" Text="Submit" Width="75px"
    onclick="SubmitButton_Click" />
    <br /><br />
    <asp:RegularExpressionValidator
    ID="RegularExpressionValidator1" runat="server" 
        ErrorMessage="*Please enter a valid email address." Display="Dynamic" ForeColor="#CC0000" 
        ControlToValidate="EmailTextBox" 
        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*Please enter a valid email address.</asp:RegularExpressionValidator>

    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
        Display="Dynamic" ErrorMessage="*Please enter an email address." 
        ForeColor="#CC0000" ControlToValidate="EmailTextBox">*Please enter an email address.</asp:RequiredFieldValidator>

    <br /><br />
    <img id="dawsonImage" src="Images/dawson.png" alt="Dawson College" />
</asp:Content>
