Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        ToolStripComboBox1.Items.Clear()

        For i = 0 To Form1.GAOpcoes.NPrateleirasX - 1
            For j As Integer = 0 To Form1.GAOpcoes.NPrateleirasY - 1
                ToolStripComboBox1.Items.Add("Prateleira: " & j & " , " & i)
            Next
        Next

    End Sub


    Private Sub atualiza(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged

        ListView1.Items.Clear()
        ListView1.Columns.Clear()

        Dim i, j, k As Integer

        For i = 0 To Form1.GAOpcoes.NLinhasPrat - 1
            ListView1.Columns.Add(CStr(i + 1), 27)
        Next

        k = ToolStripComboBox1.SelectedIndex

        Dim quociente, resto As Integer

        quociente = Math.DivRem(k, Form1.GAOpcoes.NPrateleirasX, resto)

        For j = 0 To Form1.GAOpcoes.NLinhasPrat - 1
            Dim LVItem As New ListViewItem
            LVItem.Text = CStr(Form1.Populacao(quociente * Form1.GAOpcoes.NLinhasPrat)(resto * Form1.GAOpcoes.NColunasPrat + j)(Form1.MelhorIndividuo))

            ListView1.Items.Add(LVItem)

            For k = 1 To Form1.GAOpcoes.NColunasPrat - 1
                LVItem.SubItems.Add(Form1.Populacao(quociente * Form1.GAOpcoes.NLinhasPrat + k)(resto * Form1.GAOpcoes.NColunasPrat + j)(Form1.MelhorIndividuo))
            Next


            'ListBox1.Items.Add(Populacao(0)(j)(i) & "," & Populacao(1)(j)(i) & "," & Populacao(2)(j)(i) & "," & Populacao(3)(j)(i) & "," & Populacao(4)(j)(i) & "," & Populacao(5)(j)(i) & "," & Populacao(6)(j)(i) & "," & Populacao(7)(j)(i) & "," & Populacao(8)(j)(i) & "," & Populacao(9)(j)(i) & "," & Populacao(10)(j)(i) & "," & Populacao(11)(j)(i) & "," & Populacao(12)(j)(i) & "," & Populacao(13)(j)(i) & "," & Populacao(14)(j)(i) & "," & Populacao(15)(j)(i) & "," & Populacao(16)(j)(i) & "," & Populacao(17)(j)(i) & "," & Populacao(18)(j)(i) & "," & Populacao(19)(j)(i))
        Next
    End Sub

End Class