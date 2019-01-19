Public Class VerusEZCPUPoolMiner
    Private strResults As String
    Private intStop As Integer
    Private swReader As System.IO.StreamReader
    Private thrdCMD As System.Threading.Thread
    Private Delegate Sub UpdateOut()
    Private uFin As New UpdateOut(AddressOf UpdateText)
    Private Server As String = ""
    Private Address As String = ""
    Private Worker As String = ""
    Private WorkerLead As String = ""
    Private ThreadsAndBlocksCommand As String = ""
    Private CPUThreads As Integer = 0

    Private Sub UpdateText()
        MinerConsole.Text += strResults
        MinerConsole.SelectionStart = MinerConsole.TextLength - 1
        MinerConsole.Focus()
        intStop = MinerConsole.SelectionStart
        MinerConsole.ScrollToCaret()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ValidationError As Boolean = False
        Dim ErrorMessage As String = "The Following Errors were found:" & vbNewLine
        If String.IsNullOrEmpty(AddressField.Text) = False Then
            Address = AddressField.Text
        Else
            ValidationError = True
            ErrorMessage += "-Address field is empty" & vbNewLine
        End If
        If String.IsNullOrEmpty(WorkerField.Text) = False Then
            Worker = WorkerField.Text
            WorkerLead = "."
        Else
            Worker = ""
            WorkerLead = ""
        End If
        If String.IsNullOrEmpty(ServerField.Text) = False Then
            Server = ServerField.Text
        Else
            ValidationError = True
            ErrorMessage += "-Server field is empty" & vbNewLine
        End If
        If String.IsNullOrEmpty(Threads.Text) = False Then
                If IsNumeric(Threads.Text) Then
                    If Threads.Text = "0" = False Then
                        CPUThreads = Threads.Text
                    Else
                        ValidationError = True
                        ErrorMessage += "-CPU Threads cannot be zero" & vbNewLine
                    End If
                Else
                    ValidationError = True
                    ErrorMessage += "-CPU Threads is not numeric" & vbNewLine
                End If
            Else
                ValidationError = True
                ErrorMessage += "-CPU Threads is empty" & vbNewLine
            End If
            If ValidationError = True Then
            MessageBox.Show(ErrorMessage & "Please fix these errors in order to start mining Verus")
        Else
            Dim writer As New System.IO.StreamWriter("config.conf", False)
            writer.WriteLine("Address=" & Address)
            writer.WriteLine("Worker=" & Worker)
            writer.WriteLine("Server=" & Server)
            writer.WriteLine("CPUThreads=" & CPUThreads)
            writer.WriteLine("ShowCMD=" & ShowCMD.Checked)
            writer.Close()
            thrdCMD = New System.Threading.Thread(AddressOf Miner)
            thrdCMD.IsBackground = True
            thrdCMD.Start()
            Button2.Enabled = True
            Button1.Enabled = False
        End If
    End Sub
    Private Sub Miner()
        Dim ArgumentsToUse As String = " -v " & "-l " & Server & " -u " & Address & WorkerLead & Worker & " -t " & CPUThreads & "%1 %2 %3 %4 %5 %6 %7 %8 %9"
        Dim procMiner As New Process
        If ShowCMD.Checked Then
            procMiner.StartInfo.RedirectStandardOutput = False
            procMiner.StartInfo.CreateNoWindow = False
            procMiner.StartInfo.UseShellExecute = True
            procMiner.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory & "\nheqminer.exe"
            procMiner.StartInfo.Arguments = ArgumentsToUse
            procMiner.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory & "\"
            procMiner.Start()
        Else
            AddHandler procMiner.OutputDataReceived, AddressOf MinerOutput
            procMiner.StartInfo.RedirectStandardOutput = True
            procMiner.StartInfo.RedirectStandardError = False
            procMiner.StartInfo.CreateNoWindow = True
            procMiner.StartInfo.UseShellExecute = False
            procMiner.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory & "\nheqminer.exe"
            procMiner.StartInfo.Arguments = ArgumentsToUse
            procMiner.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory & "\"
            procMiner.Start()
            procMiner.BeginOutputReadLine()
            Do Until (procMiner.HasExited)
            Loop
            procMiner.Dispose()
        End If
    End Sub
    Private Sub MinerOutput(ByVal Sender As Object, ByVal OutputLine As DataReceivedEventArgs)
        strResults = OutputLine.Data & Environment.NewLine
        Try
            Invoke(uFin)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub VerusEZCPUPoolMiner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Dim line As String
        Dim getdata As String()
        Dim result As String
        If My.Computer.FileSystem.FileExists(System.IO.Directory.GetCurrentDirectory & "\nheqminer.exe") = False Then
            MessageBox.Show("nheqminer not found. Please download the miner and put it inside a folder called ""nheqminer""")
            Process.Start("https://github.com/VerusCoin/nheqminer/releases")
            Me.Close()
        End If
        Try
            Using reader As New System.IO.StreamReader("config.conf")
                While Not reader.EndOfStream
                    line = reader.ReadLine()
                    If line.Contains("Address") Then
                        getdata = line.Split("=")
                        result = getdata(1)
                        AddressField.Text = result
                    ElseIf line.Contains("Worker") Then
                        getdata = line.Split("=")
                        result = getdata(1)
                        WorkerField.Text = result
                    ElseIf line.Contains("Server") Then
                        getdata = line.Split("=")
                        result = getdata(1)
                        ServerField.Text = result
                    ElseIf line.Contains("CPUThreads") Then
                        getdata = line.Split("=")
                        result = getdata(1)
                        Threads.Text = result
                    ElseIf line.Contains("ShowCMD") Then
                        getdata = line.Split("=")
                        result = getdata(1)
                        If result = "True" Then
                            ShowCMD.Checked = True
                        Else
                            ShowCMD.Checked = False
                        End If
                    End If
                End While
            End Using
        Catch ex As Exception
            ServerField.Text = ""
        End Try
    End Sub

    Private Sub VerusEZCPUPoolMiner_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        For Each P As Process In System.Diagnostics.Process.GetProcessesByName("nheqminer")
            P.Kill()
        Next
    End Sub

    Private Sub VerusEZCPUPoolMiner_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            Me.Hide()
            NotifyIcon1.BalloonTipTitle = "Verus EZ CPU Pool Miner"
            NotifyIcon1.BalloonTipText = "Verus EZ CPU Pool Miner running on System Tray"
            NotifyIcon1.ShowBalloonTip(5000)
        End If
    End Sub
    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://veruscoin.io")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each P As Process In System.Diagnostics.Process.GetProcessesByName("nheqminer")
            P.Kill()
        Next
        Button1.Enabled = True
        Button2.Enabled = False
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub GroupBox3_Enter(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Threads_TextChanged(sender As Object, e As EventArgs) Handles Threads.TextChanged

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub
End Class
