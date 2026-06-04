Imports System.Drawing
Imports System.Drawing.Drawing2D
Public Class Form1
    Dim State As Integer = 0
    Dim Counter As Integer = 0
    Dim PedestrianRequest As Boolean = False

    Dim EmergencyMode As Boolean = False
    Dim EmergencyDirection As String = ""
    Dim CurrentPedestrian As String = ""

    Dim TrafficNS() As String = {
    "NS", "NS1", "NS2", "NS3", "NS4", "NS5",
    "NS9", "NS10", "NS12", "NS13",
    "NS16", "NS17", "NS18"
    }

    Dim PedestrianLights() As String = {
    "PED1", "PED2", "PED3",
    "PED4", "PED5", "PED6", "PED7",
    "EW", "EW1", "EW2", "EW3",
    "EW4", "EW5", "EW6", "EW7"
    }
    Private Sub MakePanelCircular(pnl As Panel)

        Dim gp As New Drawing2D.GraphicsPath

        gp.AddEllipse(0, 0, pnl.Width - 1, pnl.Height - 1)

        pnl.Region = New Region(gp)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Timer1.Interval = 1000

        State = 0
        Counter = 0
        PedestrianRequest = False

        UpdateLights()

        For Each ctrl As Control In Me.Controls

            If TypeOf ctrl Is Panel Then

                If ctrl.Name.Contains("_Red") OrElse
                   ctrl.Name.Contains("_Yellow") OrElse
                   ctrl.Name.Contains("_Green") Then

                    MakePanelCircular(CType(ctrl, Panel))

                End If

            End If

        Next

    End Sub

    Private Sub MakeCircle(lbl As Label)

        Dim gp As New Drawing2D.GraphicsPath()
        gp.AddEllipse(0, 0, lbl.Width, lbl.Height)
        lbl.Region = New Region(gp)
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ButtonPed_Click(sender As Object, e As EventArgs) _
Handles btnPed1.Click, btnPed2.Click, btnPed3.Click, btnPed4.Click, btnPed6.Click, btnPed7.Click, btnPed8.Click, btnPed9.Click, btnPed10.Click, btnPed11.Click, btnPed12.Click

        PedestrianRequest = True
        lblPedStatus.Text = "Pedestrian Waiting..."

        Select Case CType(sender, Button).Name

            Case "btnPed1"
                CurrentPedestrian = "PED1"

            Case "btnPed2"
                CurrentPedestrian = "PED2"

            Case "btnPed3"
                CurrentPedestrian = "PED3"

            Case "btnPed4"
                CurrentPedestrian = "PED4"

            Case "btnPed6"
                CurrentPedestrian = "PED5"

            Case "btnPed7"
                CurrentPedestrian = "PED6"

            Case "btnPed8"
                CurrentPedestrian = "PED7"

            Case "btnPed9"
                CurrentPedestrian = "EW"

            Case "btnPed10"
                CurrentPedestrian = "EW1"

            Case "btnPed11"
                CurrentPedestrian = "EW2"

            Case "btnPed12"
                CurrentPedestrian = "EW3"

        End Select

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Counter += 1

        ' State Logic

        Select Case State

            Case 0

                lblStatus.Text = "Current State: NS Green"
                If Counter >= 10 Then

                    State = 1
                    Counter = 0

                End If

            Case 1

                lblStatus.Text = "Current State: NS Yellow"
                If Counter >= 3 Then

                    State = 2
                    Counter = 0

                End If

            Case 2

                lblStatus.Text = "Current State: EW Green"
                If Counter >= 10 Then

                    State = 3
                    Counter = 0

                End If

            Case 3

                lblStatus.Text = "Current State: EW Yellow"
                If Counter >= 3 Then

                    If PedestrianRequest Then

                        State = 4

                    Else

                        State = 0

                    End If

                    Counter = 0

                End If

            Case 4
                lblPedStatus.Text = "Walk Signal Active" & CurrentPedestrian
                lblStatus.Text = "Current State: Pedestrian Crossing"

                If Counter >= 5 Then

                    PedestrianRequest = False
                    lblPedStatus.Text = "Ready"

                    State = 0

                    Counter = 0

                End If

        End Select
        UpdateLights()
        UpdateCountdown()
    End Sub

    Private Sub UpdateLights()

        If EmergencyMode Then

            EmergencyControl()
            Exit Sub

        End If

        Select Case State

            Case 0
                SetNorthSouthGreen()

            Case 1
                SetNorthSouthYellow()

            Case 2
                SetEastWestGreen()

            Case 3
                SetEastWestYellow()

            Case 4
                SetPedestrianCrossing()

        End Select

    End Sub

    Private Sub SetNorthSouthGreen()

        ResetAllLights()

        For Each light As String In TrafficNS
            SetPanelColor(light & "_Green", Color.Lime)
        Next

        For Each light As String In PedestrianLights
            SetPanelColor(light & "_Red", Color.Red)
        Next

    End Sub


    Private Sub SetEastWestGreen()
        ResetAllLights()

        For Each light As String In PedestrianLights
            SetPanelColor(light & "_Green", Color.Lime)

            SetPanelColor(light & "_Red", Color.DarkRed)
        Next

        For Each light As String In TrafficNS
            SetPanelColor(light & "_Red", Color.Red)
        Next
    End Sub

    Private Sub SetNorthSouthYellow()

        ResetAllLights()

        For Each light As String In TrafficNS
            SetPanelColor(light & "_Yellow", Color.Yellow)
        Next

        For Each light As String In PedestrianLights
            SetPanelColor(light & "_Red", Color.Red)
        Next

    End Sub
    Private Sub SetEastWestYellow()

        ResetAllLights()

        For Each light As String In TrafficNS
            SetPanelColor(light & "_Yellow", Color.Yellow)
        Next

        For Each light As String In PedestrianLights
            SetPanelColor(light & "_Red", Color.Red)
        Next

    End Sub

    Private Sub SetPanelColor(panelName As String, clr As Color)

        Dim c = Me.Controls.Find(panelName, True)

        If c.Length > 0 Then
            CType(c(0), Panel).BackColor = clr
        End If

    End Sub
    Private Sub SetPedestrianCrossing()

        ResetAllLights()

        For Each light As String In TrafficNS

            SetPanelColor(light & "_Red", Color.Red)

        Next

        For Each light As String In PedestrianLights

            SetPanelColor(light & "_Red", Color.Red)

        Next

        If CurrentPedestrian <> "" Then

            SetPanelColor(CurrentPedestrian & "_Green", Color.Lime)

        End If

    End Sub
    Private Sub ResetAllLights()

        For Each light As String In TrafficNS

            SetPanelColor(light & "_Red", Color.DarkRed)
            SetPanelColor(light & "_Yellow", Color.Olive)
            SetPanelColor(light & "_Green", Color.DarkGreen)

        Next

        For Each light As String In PedestrianLights

            SetPanelColor(light & "_Red", Color.DarkRed)
            SetPanelColor(light & "_Green", Color.DarkGreen)

        Next

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        State = 0
        Counter = 0
        PedestrianRequest = False

        UpdateLights()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Timer1.Start()
        btnStart.Enabled = False
        btnStop.Enabled = True
    End Sub


    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        Timer1.Stop()
        btnStart.Enabled = True
        btnStop.Enabled = False
    End Sub

    Private Sub MakeCircular(btn As Button)

        Dim path As New GraphicsPath()
        path.AddEllipse(0, 0, btn.Width, btn.Height)
        btn.Region = New Region(path)

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnEmergencyNorth.Click
        EmergencyMode = True
        EmergencyDirection = "North"

        UpdateLights()

    End Sub


    Private Sub btnEmergencySouth_Click(sender As Object, e As EventArgs) Handles btnEmergencySouth.Click
        EmergencyMode = True
        EmergencyDirection = "South"

        UpdateLights()

    End Sub

    Private Sub btnEmergencyEast_Click(sender As Object, e As EventArgs) Handles btnEmergencyEast.Click
        EmergencyMode = True
        EmergencyDirection = "East"

        UpdateLights()
    End Sub

    Private Sub btnEmergencyWest_Click(sender As Object, e As EventArgs) Handles btnEmergencyWest.Click
        EmergencyMode = True
        EmergencyDirection = "West"

        UpdateLights()
    End Sub

    Private Sub btnEmergencyOff_Click(sender As Object, e As EventArgs) Handles btnEmergencyOff.Click
        EmergencyMode = False
        State = 0
        Counter = 0

        UpdateLights()
    End Sub
    Private Sub UpdateCountdown()

        Dim Remaining As Integer

        Select Case State

            Case 0
                Remaining = 10 - Counter

            Case 1
                Remaining = 3 - Counter

            Case 2
                Remaining = 10 - Counter

            Case 3
                Remaining = 3 - Counter

            Case 4
                Remaining = 5 - Counter

        End Select
        label1.Text = Remaining.ToString()
        Label2.Text = Remaining.ToString()
        Label3.Text = Remaining.ToString()
        Label4.Text = Remaining.ToString()
        Label5.Text = Remaining.ToString()
        Label6.Text = Remaining.ToString()
        Label7.Text = Remaining.ToString()

    End Sub
    Private Sub EmergencyControl()

        ResetAllLights()

        Select Case EmergencyDirection

            Case "North", "South"

                For Each light As String In TrafficNS

                    SetPanelColor(light & "_Green", Color.Lime)

                Next

                For i As Integer = 0 To 7

                    Dim prefix = If(i = 0, "EW", "EW" & i)

                    SetPanelColor(prefix & "_Red", Color.Red)

                Next

            Case "East", "West"

                For i As Integer = 0 To 7

                    Dim prefix = If(i = 0, "EW", "EW" & i)

                    SetPanelColor(prefix & "_Green", Color.Lime)

                Next

                For i As Integer = 0 To 19

                    Dim prefix = If(i = 0, "NS", "NS" & i)

                    SetPanelColor(prefix & "_Red", Color.Red)

                Next

        End Select

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub label1_Click_1(sender As Object, e As EventArgs) Handles label1.Click

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles lblStatus.Click

    End Sub

    Private Sub PED1_Red_Paint(sender As Object, e As PaintEventArgs) Handles PED1_Red.Paint

    End Sub
End Class
