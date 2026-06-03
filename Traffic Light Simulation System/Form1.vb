Imports System.Drawing
Public Class Form1
    Dim State As Integer = 0
    Dim Counter As Integer = 0
    Dim PedestrianRequest As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Timer1.Interval = 1000

        State = 0
        Counter = 0
        PedestrianRequest = False

        UpdateLights()

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles lblStatus.Click

    End Sub

    Private Sub ButtonPed_Click(sender As Object, e As EventArgs) _
    Handles btnPed1.Click, btnPed2.Click, btnPed3.Click, btnPed4.Click, btnPed6.Click, btnPed7.Click, btnPed8.Click, btnPed9.Click, btnPed10.Click, btnPed11.Click, btnPed12.Click

        PedestrianRequest = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Counter += 1

        Select Case State

            Case 0
                lblStatus.Text = "North-South Green"

                If Counter >= 10 Then
                    State = 1
                    Counter = 0
                End If

            Case 1
                lblStatus.Text = "East-West Green"

                If Counter >= 8 Then

                    If PedestrianRequest Then
                        State = 2
                    Else
                        State = 0
                    End If

                    Counter = 0
                End If

            Case 2
                lblStatus.Text = "Pedestrian Crossing"

                If Counter >= 5 Then
                    PedestrianRequest = False
                    State = 0
                    Counter = 0
                End If

        End Select

        UpdateLights()

    End Sub

    Private Sub UpdateLights()

        Select Case State

            Case 0
                SetNorthSouthGreen()

            Case 1
                SetEastWestGreen()

            Case 2
                SetPedestrianCrossing()

        End Select

    End Sub

    Private Sub SetNorthSouthGreen()

        ResetAllLights()

        For i As Integer = 0 To 19

            Dim prefix As String = If(i = 0, "NS", "NS" & i)

            SetPanelColor(prefix & "_Green", Color.Lime)

        Next

        For i As Integer = 0 To 7

            Dim prefix As String = If(i = 0, "EW", "EW" & i)

            SetPanelColor(prefix & "_Red", Color.Red)

        Next

    End Sub


    Private Sub SetEastWestGreen()

        ResetAllLights()

        For i As Integer = 0 To 7

            Dim prefix As String = If(i = 0, "EW", "EW" & i)

            SetPanelColor(prefix & "_Green", Color.Lime)

        Next

        For i As Integer = 0 To 19

            Dim prefix As String = If(i = 0, "NS", "NS" & i)

            SetPanelColor(prefix & "_Red", Color.Red)

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

        For i As Integer = 0 To 19

            Dim prefix As String = If(i = 0, "NS", "NS" & i)

            SetPanelColor(prefix & "_Red", Color.Red)

        Next

        For i As Integer = 0 To 7

            Dim prefix As String = If(i = 0, "EW", "EW" & i)

            SetPanelColor(prefix & "_Red", Color.Red)

        Next

    End Sub
    Private Sub ResetAllLights()

        For i As Integer = 0 To 19

            Dim prefix As String = If(i = 0, "NS", "NS" & i)

            SetPanelColor(prefix & "_Red", Color.DarkRed)
            SetPanelColor(prefix & "_Yellow", Color.Olive)
            SetPanelColor(prefix & "_Green", Color.DarkGreen)

        Next

        For i As Integer = 0 To 7

            Dim prefix As String = If(i = 0, "EW", "EW" & i)

            SetPanelColor(prefix & "_Red", Color.DarkRed)
            SetPanelColor(prefix & "_Yellow", Color.Olive)
            SetPanelColor(prefix & "_Green", Color.DarkGreen)

        Next

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        State = 0
        Counter = 0
        PedestrianRequest = False

        lblStatus.Text = "System Reset"
        UpdateLights()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Timer1.Start()
    End Sub


    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        Timer1.Stop()
    End Sub

End Class
