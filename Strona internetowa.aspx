<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Strona internetowa.aspx.cs" Inherits="Strona_internetowa" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Filtr pasywny by Krzysztof Grzywiński</title>
    <link rel="stylesheet" type="text/css" href="StronaStyleSheet.css" />
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="idPanel" runat="server" CssClass="titlePanel">
                <h1>Politechnika Gdańska</h1>
                <h2>Wydział Elektortechniki i Automatyki</h2>
                <h3>Programowanie aplikacji internetowych</h3>
                <h4>Krzysztof Grzywiński,  Nr. albumu: 149969</h4>
            </asp:Panel>
            <br />

            <asp:Panel ID="drawingPanel" runat="server" GroupingText="Rysunek schematu ideowego filtru pasywnego" CssClass="Panel">
                <asp:Image ID="filter" runat="server" AlternateText="Brak odpowiedniego rysunku" CssClass="Image" Width="524px" Height="198px" src="układFiltr.png" />
            </asp:Panel>
            <br />

            <asp:Panel ID="aboutElementsPanel" runat="server" GroupingText="Opis elementów ukladu" CssClass="Panel">
                <asp:Label ID="labAboutU1" runat="server" Text="U1 - napięcie zasilania/wejściowe filtru"></asp:Label>
                <br />
                <asp:Label ID="labAboutU2" runat="server" Text="U2 - napięcie wyjściowe filtru"></asp:Label>
                <br />
                <asp:Label ID="labAboutL" runat="server" Text="L - cewka"></asp:Label>
                <br />
                <asp:Label ID="labAboutC" runat="server" Text="C - kondensator"></asp:Label>
                <br />
                <asp:Label ID="labAboutR1" runat="server" Text="R1 - rezystor"></asp:Label>
                <br />
                <asp:Label ID="labAboutR2" runat="server" Text="R2 - rezystor"></asp:Label>
            </asp:Panel>
            <br />

            <asp:Panel ID="elemParamPanel" runat="server" GroupingText="Edycja paramterów filtru" CssClass="Panel">
                <asp:Label ID="labR1" runat="server" Text="Rezystor R1 [Ohm]:"></asp:Label>
                <asp:TextBox ID="txtR1" runat="server" Text="1" CssClass="TextBox"></asp:TextBox>
                <br />
                <asp:Label ID="labR2" runat="server" Text="Rezystor R2 [Ohm]:"></asp:Label>
                <asp:TextBox ID="txtR2" runat="server" Text="100" CssClass="TextBox"></asp:TextBox>
                <br />
                <asp:Label ID="labL" runat="server" Text="Cewka L [mH]:"></asp:Label>
                <asp:TextBox ID="txtL" runat="server" Text="3" CssClass="TextBox"></asp:TextBox>
                <br />
                <asp:Label ID="labC1" runat="server" Text="Kondensator C [uF]:"></asp:Label>
                <asp:TextBox ID="txtC" runat="server" Text="100" CssClass="TextBox"></asp:TextBox>
            </asp:Panel>

            <br />
            <asp:Panel ID="voltageParamPanel" runat="server" CssClass="Panel" GroupingText="Edycja paramterów napięcia zasilającego U1 oraz zakresu częstotliwości">
                <asp:Label ID="voltU1" runat="server" Text="Amplituda napięcia U1 [V]:"></asp:Label>
                <asp:TextBox ID="txtVoltU1" runat="server" Text="100" CssClass="TextBox"></asp:TextBox>
                <br />
                <asp:Label ID="freqU1" runat="server" Text="Częstotliwość napięcia zasilającego [Hz]:"></asp:Label>
                <asp:TextBox ID="txtFreqU1" runat="server" Text="50" CssClass="TextBox"></asp:TextBox>
                <br />
                <asp:Label ID="minRangeFreq" runat="server" Text="Dolna granica zakresu częstotliwości f [Hz]:"></asp:Label>
                <asp:TextBox ID="txtMinRangeFreq" runat="server" Text="1" CssClass="TextBox"></asp:TextBox>
                <br />
                <asp:Label ID="maxRangeFreq" runat="server" Text="Górna granica zakresu częstotliwości f [Hz]:"></asp:Label>
                <asp:TextBox ID="txtMaxRangeFreq" runat="server" Text="10000" CssClass="TextBox"></asp:TextBox>
            </asp:Panel>
            <br />
            <asp:Button ID="drawButton" runat="server" Text="Rysuj przebiegi" CssClass="Button" OnClick="drawButton_Click"  />
            <br />
            <asp:Label ID="messageLabel" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <br />

        <asp:MultiView runat="server" ID="MultiView1" ActiveViewIndex="0">
            <asp:View ID="widmoAmplitudowe" runat="server">
                <asp:Label ID="labWidomoAmplitudowe1" runat="server" Text="Widmo amplitudowe" CssClass="Inactive"></asp:Label>
                <asp:LinkButton ID="labWidmoFazowe1" runat="server" Text="Widmo fazowe" CssClass="Active" OnClick="drawButton_Click" CommandName="WidmoFazowe" OnCommand="multiView_Command"></asp:LinkButton>
                <asp:LinkButton ID="labObwiedniaPradu1" runat="server" Text="Obwiednia prądu zasilajacego" CssClass="Active" OnClick="drawButton_Click" CommandName="ObwiedniaPradu" OnCommand="multiView_Command"></asp:LinkButton>
                <br />
                <br />
                <asp:Panel ID="chartsPanel1" runat="server"  CssClass="Panel" >
                    <asp:Chart ID="Chart1" runat="server" Width="305px" BackImage="/background2.png">
                        <Series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                            <asp:Legend Name="Legend1"></asp:Legend>
                        </Legends>
                    </asp:Chart>
                </asp:Panel>
            </asp:View>
            <asp:View ID="widmoFazowe" runat="server">
                <asp:LinkButton ID="labWidmoAmplitudowe2" runat="server" Text="Widmo amplitudowe" CssClass="Active" OnClick="drawButton_Click" CommandName="WidmoAmplitudowe" OnCommand="multiView_Command"></asp:LinkButton>
                <asp:Label ID="labWidmoFazowe2" runat="server" Text="Widmo fazowe" CssClass="Inactive"></asp:Label>
                <asp:LinkButton ID="labObwiedniaPradu2" runat="server" Text="Obwiednia prądu zasilajacego" CssClass="Active" OnClick="drawButton_Click" CommandName="ObwiedniaPradu" OnCommand="multiView_Command"></asp:LinkButton>
                <br />
                <br />
                <asp:Panel ID="chartsPanel2" runat="server" CssClass="Panel" >
                    <asp:Chart ID="Chart2" runat="server" BackImage="/background2.png">
                        <Series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                            <asp:Legend Name="Legend1"></asp:Legend>
                        </Legends>
                    </asp:Chart>
                </asp:Panel>
            </asp:View>

            <asp:View ID="obwiedniaPradu" runat="server">
                <asp:LinkButton ID="labWidmoAmplitudowe3" runat="server" Text="Widmo amplitudowe" CssClass="Active" OnClick="drawButton_Click" CommandName="WidmoAmplitudowe" OnCommand="multiView_Command"></asp:LinkButton>
                <asp:LinkButton ID="labWidmoFazowe3" runat="server" Text="Widmo fazowe" CssClass="Active" OnClick="drawButton_Click" CommandName="WidmoFazowe" OnCommand="multiView_Command"></asp:LinkButton>
                <asp:Label ID="labObwiedniaPradu3" runat="server" Text="Obwiednia prądu zasilajacego" CssClass="Inactive" ></asp:Label>
                <br />
                <br />
                <asp:Panel ID="chartPanel3" runat="server" CssClass="Panel">
                    <asp:Chart ID="Chart3" runat="server" BackImage="/background2.png">
                        <Series>
                            <asp:Series Name="Series1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                        <Legends>
                            <asp:Legend Name="Legend1"></asp:Legend>
                        </Legends>
                    </asp:Chart>
                </asp:Panel>
            </asp:View>
        </asp:MultiView>
        <br />
    </form>
</body>
</html>
