Imports Emgu.CV
Imports Emgu.CV.UI
Imports Emgu.CV.Structure
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO

Public Class Form1
    Private fatordim As Double = 1
    Private Nprod As Integer = 0
    Private CVOpcoes As New Navigator
    Public GAOpcoes As New GAProp
    Public incapaz As Integer = 0
    Public Populacao()()() As Integer
    Public MelhorIndividuo As Integer
    Private MeanFitness() As Double
    Private BestFitness As Double
    Private _capture As Capture
    Private _capture2 As Capture
    Private cont, geracao As Integer
    Private xrange As Double = 0, yrange As Double = 0, zrange As Double = 0
    Private dimensao(2) As Double
    Private Prod(0)() As Integer
    Private camera As Integer = 0
    Public MSizeX As Integer = GAOpcoes.NPrateleirasX * GAOpcoes.NLinhasPrat - 1
    Public MSizeY As Integer = GAOpcoes.NPrateleirasY * GAOpcoes.NColunasPrat - 1
    Public MSizeZ As Integer = GAOpcoes.Populacao - 1
    Private dados As IO.StreamWriter

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize()
        Me.Text = My.Application.Info.Title

        ToolStripComboBox2.SelectedIndex = 0
        ToolStripComboBox3.SelectedIndex = 2

        cont = 0

        PropertyGrid1.SelectedObject = CVOpcoes
        PropertyGrid2.SelectedObject = GAOpcoes

        carregar()
        TextBox4.Text = Nprod

    End Sub

    Private Sub form1_exit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.FormClosing

        If IO.File.Exists("D:\Caue\TD\TFG - Cam\data.txt") Then
            IO.File.Delete("D:\Caue\TD\TFG - Cam\data.txt")
        End If

        dados = New IO.StreamWriter("D:\Caue\TD\TFG - Cam\data.txt")
        For i = 0 To Nprod - 1
            dados.WriteLine(Prod(i)(0) & ";" & Prod(i)(1) & ";" & Prod(i)(2) & ";" & Prod(i)(3) & ";" & Prod(i)(4))
        Next

        dados.Close()

    End Sub

    Private Sub ProcessFrame(ByVal sender As Object, ByVal arg As EventArgs)

        Dim Lower As New Bgr(CVOpcoes.Cor1.B, CVOpcoes.Cor1.G, CVOpcoes.Cor1.R)
        Dim Higher As New Bgr(CVOpcoes.Cor2.B, CVOpcoes.Cor2.G, CVOpcoes.Cor2.R)

        Dim frame As Image(Of Bgr, [Byte]) = _capture.QueryFrame()
        Dim frame2 As Image(Of Bgr, [Byte]) = _capture2.QueryFrame()
       
        Dim grayFrame As Image(Of Gray, [Byte]) = frame.InRange(Lower, Higher)
        Dim grayFrame2 As Image(Of Gray, [Byte]) = frame2.InRange(Lower, Higher)

        Dim cannyFrame As Image(Of Gray, [Byte]) = grayFrame.Canny(New Gray(CVOpcoes.ThresCanny), New Gray(CVOpcoes.HoughMinLine))
        Dim cannyFrame2 As Image(Of Gray, [Byte]) = grayFrame2.Canny(New Gray(CVOpcoes.ThresCanny), New Gray(CVOpcoes.HoughMinLine))

        Dim Lines() As LineSegment2D = cannyFrame.HoughLinesBinary(CVOpcoes.HoughRHO, CVOpcoes.HoughTheta, CVOpcoes.HoughThres, CVOpcoes.HoughMinLine, CVOpcoes.HoughGapLines)(0)
        Dim Lines2() As LineSegment2D = cannyFrame2.HoughLinesBinary(CVOpcoes.HoughRHO, CVOpcoes.HoughTheta, CVOpcoes.HoughThres, CVOpcoes.HoughMinLine, CVOpcoes.HoughGapLines)(0)

        For Each Line As LineSegment2D In Lines
            Dim NewLine As New LineSegment2D(New Point(Line.P1.X, Line.P1.Y), New Point(Line.P2.X, Line.P2.Y))
            frame.Draw(NewLine, New Bgr(Color.Red), 1)

            If (Math.Abs(Line.P1.X - Line.P2.X) > Math.Abs(Line.P1.Y - Line.P2.Y)) Then
                If Line.Length > xrange Then
                    xrange = Line.Length
                End If
            Else
                If Line.Length > zrange Then
                    zrange = Line.Length
                End If
            End If

        Next

        For Each Line2 As LineSegment2D In Lines2
            Dim NewLine2 As New LineSegment2D(New Point(Line2.P1.X, Line2.P1.Y), New Point(Line2.P2.X, Line2.P2.Y))
            frame2.Draw(NewLine2, New Bgr(Color.Red), 1)
            If (Math.Abs(Line2.P1.X - Line2.P2.X) < Math.Abs(Line2.P1.Y - Line2.P2.Y)) Then
                If Line2.Length > yrange Then
                    yrange = Line2.Length
                End If
            End If
        Next

        Select Case ToolStripComboBox2.SelectedIndex
            Case 0
                ImgBox.Image = frame
                ImageBox1.Image = frame2
            Case 1
                ImgBox.Image = grayFrame
                ImageBox1.Image = grayFrame2
            Case 2
                ImgBox.Image = cannyFrame
                ImageBox1.Image = cannyFrame2
            Case Else

        End Select

    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click

        If _capture Is Nothing Then
            Try
                _capture = New Capture(0)
                _capture.SetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640)
                _capture.SetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480)
                _capture2 = New Capture(1)
                _capture2.SetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640)
                _capture2.SetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480)
                AddHandler Application.Idle, AddressOf ProcessFrame
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            _capture = Nothing
            _capture2 = Nothing
            RemoveHandler Application.Idle, AddressOf ProcessFrame
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Nprod += 1

        Dim aux1(Nprod)() As Integer
        For i = 0 To Nprod
            ReDim aux1(i)(4)
        Next

        For i = 0 To Nprod - 1
            For j = 0 To 4
                aux1(i)(j) = Prod(i)(j)
            Next
        Next

        ReDim Prod(Nprod)
        For i = 0 To Nprod
            ReDim Prod(i)(4)
        Next

        For i = 0 To Nprod
            For j = 0 To 4
                Prod(i)(j) = aux1(i)(j)
            Next
        Next


        Prod(Nprod - 1)(0) = Nprod
        Prod(Nprod - 1)(1) = CInt(xrange * fatordim)
        Prod(Nprod - 1)(2) = CInt(yrange * fatordim)
        Prod(Nprod - 1)(3) = CInt(zrange * fatordim)
        Prod(Nprod - 1)(4) = ToolStripComboBox3.SelectedIndex + 1

        TextBox3.Text = Prod(Nprod - 1)(3)
        TextBox1.Text = Prod(Nprod - 1)(1)
        TextBox2.Text = Prod(Nprod - 1)(2)


        xrange = 0
        yrange = 0
        zrange = 0
        TextBox4.Text = Nprod




    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click

        Dim newpopulacao()()() As Integer

        Dim i, j As Integer
        Dim pai, mae, cont As Integer
        Dim Fitness() As Double

        incapaz = 0
        MSizeX = GAOpcoes.NPrateleirasX * GAOpcoes.NLinhasPrat - 1
        MSizeY = GAOpcoes.NPrateleirasY * GAOpcoes.NColunasPrat - 1
        MSizeZ = GAOpcoes.Populacao - 1

        PropertyGrid2.Enabled = False

        testecapacidade()

        If incapaz = 1 Then
            PropertyGrid2.Enabled = True
            Exit Sub
        End If

        ReDim MeanFitness(GAOpcoes.Geracoes - 1)

        ReDim Populacao(MSizeX)
        ReDim newpopulacao(MSizeX)
        For i = 0 To MSizeX
            ReDim Populacao(i)(MSizeY)
            ReDim newpopulacao(i)(MSizeY)
        Next

        For i = 0 To MSizeX
            For j = 0 To MSizeY
                ReDim Populacao(i)(j)(MSizeZ)
                ReDim newpopulacao(i)(j)(MSizeZ)
            Next
        Next

        ReDim Fitness(MSizeZ)

        aginicializar(Populacao, MSizeX, MSizeY, MSizeZ)
        i = 0


        Fitness = checkers(Populacao, MSizeX, MSizeY, MSizeZ, Fitness)

        geracao = 0
        BestFitness = Fitness.Min
        MeanFitness(geracao) = Fitness.Sum / (MSizeZ + 1)



        cont = 0

        geracao = 1
        While geracao < GAOpcoes.Geracoes
            i = 0
            pai = fpai(Fitness, BestFitness, MSizeZ)
            newpopulacao = atualiza(Populacao, newpopulacao, pai, i)
            For i = 1 To MSizeZ
                mae = Rnd(1) * MSizeZ
                If Rnd(1) < (1 - GAOpcoes.ProMutacao / 100) Then
                    newpopulacao = cruzamento(Populacao, newpopulacao, pai, mae, i)
                Else
                    newpopulacao = mutacao(Populacao, newpopulacao, pai, i)
                End If
            Next

            Populacao = newpopulacao

            Fitness = checkers(Populacao, MSizeX, MSizeY, MSizeZ, Fitness)

            Dim nBestFitness As Double = Fitness.Min

            If nBestFitness = BestFitness Then
                cont = cont + 1
            Else
                cont = 0
                BestFitness = nBestFitness
            End If

            MeanFitness(geracao) = Fitness.Sum / (MSizeZ + 1)

            If cont >= 20 Then
                Exit While
            End If

            geracao = geracao + 1



        End While

        PropertyGrid2.Enabled = True
        Form2.Show()

    End Sub

    Private Sub carregar()
        Dim fluxoTexto As IO.StreamReader
        Dim linhaTexto As String
        Dim delimitador As String = ";"
        Dim i As Integer = 0
        Dim j, k As Integer

        ReDim Prod(0)(4)

        k = 0

        If IO.File.Exists("D:\Caue\TD\TFG - Cam\data.txt") Then

            fluxoTexto = New IO.StreamReader("D:\Caue\TD\TFG - Cam\data.txt")
            linhaTexto = fluxoTexto.ReadLine



            While linhaTexto <> Nothing

                Dim conteudoLinha = Split(linhaTexto, delimitador)

                For i = 0 To UBound(conteudoLinha)
                    Prod(k)(i) = CInt(conteudoLinha(i))
                Next

                Nprod += 1
                k += 1
                linhaTexto = fluxoTexto.ReadLine

                Dim aux1(Nprod)() As Integer
                For i = 0 To Nprod
                    ReDim aux1(i)(4)
                Next

                For i = 0 To Nprod - 1
                    For j = 0 To 4
                        aux1(i)(j) = Prod(i)(j)
                    Next
                Next

                ReDim Prod(Nprod)
                For i = 0 To Nprod
                    ReDim Prod(i)(4)
                Next

                For i = 0 To Nprod
                    For j = 0 To 4
                        Prod(i)(j) = aux1(i)(j)
                    Next
                Next
            End While
            fluxoTexto.Close()
        End If

    End Sub

    Private Function checkers(ByVal individuo()()() As Integer, ByVal SizeX As Integer, ByVal SizeY As Integer, ByVal SizeZ As Integer, ByVal fitness() As Double)

        Dim i, j, k, l As Integer
        For l = 0 To SizeZ

            checkers = 0
            For i = 0 To SizeX
                For j = 0 To SizeY
                    For k = 0 To Nprod - 1
                        If Prod(k)(0) = individuo(i)(j)(l) Then
                            checkers = checkers + Prod(k)(4) * (j + 1)
                        End If
                    Next
                Next
            Next
            fitness(l) = checkers

        Next
        Return fitness
    End Function

    Private Function aginicializar(ByVal populacao()()() As Integer, ByVal SizeX As Integer, ByVal SizeY As Integer, ByVal SizeZ As Integer)
        Dim i, j, k, l, ix, iy, contx, conty As Integer

        Dim a, b, c, d, e, f, g As Integer

        For i = 0 To SizeX
            For j = 0 To SizeY
                For k = 0 To SizeZ
                    populacao(i)(j)(k) = 0
                Next
            Next
        Next

        For j = 0 To SizeZ

            i = 1

            While i <= Nprod
                d = 0
                ix = (Rnd(1) * (GAOpcoes.NPrateleirasX - 1))
                iy = (Rnd(1) * (GAOpcoes.NPrateleirasY - 1))



                For a = 0 To GAOpcoes.NLinhasPrat - 1
                    For b = 0 To GAOpcoes.NColunasPrat - 1
                        contx = 0
                        conty = 0
                        If populacao(ix * GAOpcoes.NLinhasPrat + a)(iy * GAOpcoes.NColunasPrat + b)(j) = 0 Then
                            c = b
                            f = a
                            While c < GAOpcoes.NColunasPrat
                                If populacao(ix * GAOpcoes.NLinhasPrat + a)(iy * GAOpcoes.NColunasPrat + c)(j) = 0 Then
                                    conty += 1
                                    c += 1
                                Else
                                    Exit While
                                End If
                            End While

                            While f < GAOpcoes.NLinhasPrat
                                If populacao(ix * GAOpcoes.NLinhasPrat + f)(iy * GAOpcoes.NColunasPrat + b)(j) = 0 Then
                                    contx += 1
                                    f += 1
                                Else
                                    Exit While
                                End If
                            End While

                            If contx >= Prod(i - 1)(1) And conty >= Prod(i - 1)(3) Then
                                For k = 0 To Prod(i - 1)(1) - 1
                                    For l = 0 To Prod(i - 1)(3) - 1
                                        populacao(ix * GAOpcoes.NLinhasPrat + a + k)(iy * GAOpcoes.NColunasPrat + b + l)(j) = i
                                    Next
                                Next
                                i = i + 1
                                d = 1
                                Exit For

                            End If

                        End If
                    Next
                    If d = 1 Then
                        Exit For
                    End If
                Next


            End While
        Next
        Return populacao
    End Function

    Private Function fpai(ByVal fitness() As Double, ByVal bestfitness As Double, ByVal sizey As Integer)

        Dim i, ipai As Integer
        For i = 0 To sizey
            If fitness(i) = bestfitness Then
                ipai = i
            End If
        Next
        Return ipai

    End Function

    Private Function atualiza(ByVal populacao()()() As Integer, ByVal newpopulacao()()() As Integer, ByVal ant As Integer, ByVal index As Integer)
        Dim i, j As Integer
        For i = 0 To MSizeX
            For j = 0 To MSizeY
                newpopulacao(i)(j)(index) = populacao(i)(j)(ant)
            Next
        Next
        Return newpopulacao
    End Function

    Private Function mutacao(ByVal populacao()()() As Integer, ByVal newpopulacao()()() As Integer, ByVal individuo As Integer, ByVal index As Integer)
        Dim x1, x2, y1, y2, i, j As Integer
        Dim aux()() As Integer

        newpopulacao = atualiza(populacao, newpopulacao, individuo, index)

        ReDim aux(GAOpcoes.NLinhasPrat - 1)
        For i = 0 To GAOpcoes.NLinhasPrat - 1
            ReDim aux(i)(GAOpcoes.NColunasPrat - 1)
        Next

        x1 = Rnd(1) * (GAOpcoes.NPrateleirasX - 1)
        x2 = Rnd(1) * (GAOpcoes.NPrateleirasX - 1)
        y1 = Rnd(1) * (GAOpcoes.NPrateleirasY - 1)
        y2 = Rnd(1) * (GAOpcoes.NPrateleirasY - 1)

        While x1 = x2 And y1 = y2
            x1 = Rnd(1) * (GAOpcoes.NPrateleirasX - 1)
            x2 = Rnd(1) * (GAOpcoes.NPrateleirasX - 1)
            y1 = Rnd(1) * (GAOpcoes.NPrateleirasY - 1)
            y2 = Rnd(1) * (GAOpcoes.NPrateleirasY - 1)
        End While

        For i = 0 To GAOpcoes.NLinhasPrat - 1
            For j = 0 To GAOpcoes.NColunasPrat - 1
                aux(i)(j) = newpopulacao(x1 * GAOpcoes.NLinhasPrat + i)(y1 * GAOpcoes.NColunasPrat + j)(index)
            Next
        Next

        For i = 0 To GAOpcoes.NLinhasPrat - 1
            For j = 0 To GAOpcoes.NColunasPrat - 1
                newpopulacao(x1 * GAOpcoes.NLinhasPrat + i)(y1 * GAOpcoes.NColunasPrat + j)(index) = newpopulacao(x2 * GAOpcoes.NLinhasPrat + i)(y2 * GAOpcoes.NColunasPrat + j)(index)
            Next
        Next

        For i = 0 To GAOpcoes.NLinhasPrat - 1
            For j = 0 To GAOpcoes.NColunasPrat - 1
                newpopulacao(x2 * GAOpcoes.NLinhasPrat + i)(y2 * GAOpcoes.NColunasPrat + j)(index) = aux(i)(j)
            Next
        Next

        Return newpopulacao

    End Function

    Private Function cruzamento(ByVal populacao()()() As Integer, ByVal newpopulacao()()() As Integer, ByVal pai As Integer, ByVal mae As Integer, ByVal index As Integer)

        Dim pto As Integer = Rnd(1) * (GAOpcoes.NPrateleirasY - 1)

        While pto = 0 Or pto = GAOpcoes.NPrateleirasY - 1
            pto = Rnd(1) * (GAOpcoes.NPrateleirasY - 1)
        End While

        Dim i, j, k, l, cont, a, b, c, d, e, f, g, ix, iy, contx, conty, aux, z As Integer
        Dim duplo(Nprod - 1) As Integer
        Dim falta(Nprod - 1) As Integer

        e = 0
        g = 0

        For i = 0 To MSizeX
            For j = 0 To (MSizeY - 1) / 2
                newpopulacao(i)(j)(index) = populacao(i)(j)(pai)
            Next
        Next

        For i = 0 To MSizeX
            For j = (MSizeY + 1) / 2 To MSizeY
                newpopulacao(i)(j)(index) = populacao(i)(j)(mae)
            Next
        Next

        For k = 1 To Nprod
            cont = 0
            For i = 0 To MSizeX
                For j = 0 To MSizeY
                    If newpopulacao(i)(j)(index) = k Then
                        cont += 1
                    End If
                Next
            Next

            If cont > (Prod(k - 1)(1) * Prod(k - 1)(3)) Then
                duplo(e) = k
                e += 1
            End If

            If cont < (Prod(k - 1)(1) * Prod(k - 1)(3)) Then
                falta(g) = k
                g += 1
            End If
        Next

        For l = 0 To e - 1
            d = 0
            For i = 0 To MSizeX
                For j = 0 To MSizeY
                    If newpopulacao(i)(j)(index) = duplo(l) Then
                        For a = 0 To ((Prod(duplo(l) - 1)(1)) - 1)
                            For b = 0 To ((Prod(duplo(l) - 1)(3)) - 1)
                                newpopulacao(i + a)(j + b)(index) = 0
                            Next
                        Next
                        d = 1
                        Exit For
                    End If
                Next
                If d = 1 Then
                    Exit For
                End If
            Next
        Next

        i = 0

        While i < g
            d = 0
            ix = (Rnd(1) * (GAOpcoes.NPrateleirasX - 1))
            iy = (Rnd(1) * (GAOpcoes.NPrateleirasY - 1))

            For a = 0 To GAOpcoes.NLinhasPrat - 1
                For b = 0 To GAOpcoes.NColunasPrat - 1
                    contx = 0
                    aux = 0
                    conty = 0
                    If newpopulacao(ix * GAOpcoes.NLinhasPrat + a)(iy * GAOpcoes.NColunasPrat + b)(index) = 0 Then
                        c = b
                        f = a
                        While c < GAOpcoes.NColunasPrat
                            If newpopulacao(ix * GAOpcoes.NLinhasPrat + a)(iy * GAOpcoes.NColunasPrat + c)(index) = 0 Then
                                conty += 1
                                c += 1
                            Else
                                Exit While
                            End If
                        End While

                        While f < GAOpcoes.NLinhasPrat
                            z = 0
                            If newpopulacao(ix * GAOpcoes.NLinhasPrat + f)(iy * GAOpcoes.NColunasPrat + b)(index) = 0 Then

                                c = b

                                While c < GAOpcoes.NColunasPrat
                                    If newpopulacao(ix * GAOpcoes.NLinhasPrat + f)(iy * GAOpcoes.NColunasPrat + c)(index) = 0 Then
                                        aux += 1
                                        c += 1
                                    Else
                                        Exit While
                                    End If
                                End While

                                If aux >= conty Then
                                    contx += 1
                                Else
                                    z = 1
                                End If

                                f += 1
                            Else
                                Exit While
                            End If

                            If z = 1 Then
                                Exit While
                            End If
                        End While

                        If contx >= Prod(falta(i) - 1)(1) And conty >= Prod(falta(i) - 1)(3) Then
                            For k = 0 To Prod(falta(i) - 1)(1) - 1
                                For l = 0 To Prod(falta(i) - 1)(3) - 1
                                    newpopulacao(ix * GAOpcoes.NLinhasPrat + a + k)(iy * GAOpcoes.NColunasPrat + b + l)(index) = falta(i)
                                Next
                            Next
                            i = i + 1
                            d = 1
                            Exit For

                        End If

                    End If
                Next
                If d = 1 Then
                    Exit For
                End If
            Next


        End While

        For i = 0 To Nprod - 1
            duplo(i) = 0
            falta(i) = 0
        Next

        Return newpopulacao
    End Function

    Private Sub testecapacidade()
        Dim i, cont As Integer

        cont = 0

        For i = 0 To Nprod - 1
            cont += Prod(i)(1) * Prod(i)(3)
        Next

        If cont > (MSizeX + 1) * (MSizeY + 1) Then
            MessageBox.Show("Armazém incapaz de alocar todos os produtos.")
            incapaz = 1
        End If
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        fatordim = CVOpcoes.CalibracaoDimX / xrange
    End Sub

End Class
