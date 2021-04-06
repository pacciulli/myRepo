Public Class Navigator

    Private _ThresCanny As Double = 100
    Public Property ThresCanny() As Double
        Get
            Return _ThresCanny
        End Get
        Set(ByVal value As Double)
            _ThresCanny = value
        End Set
    End Property

    Private _HoughRHO As Double = 1
    Public Property HoughRHO() As Double
        Get
            Return _HoughRHO
        End Get
        Set(ByVal value As Double)
            _HoughRHO = value
        End Set
    End Property

    Private _HoughTheta As Double = 0.05
    Public Property HoughTheta() As Double
        Get
            Return _HoughTheta
        End Get
        Set(ByVal value As Double)
            _HoughTheta = value
        End Set
    End Property

    Private _HoughThres As Double = 20
    Public Property HoughThres() As Double
        Get
            Return _HoughThres
        End Get
        Set(ByVal value As Double)
            _HoughThres = value
        End Set
    End Property

    Private _HoughMinLine As Double = 40
    Public Property HoughMinLine() As Double
        Get
            Return _HoughMinLine
        End Get
        Set(ByVal value As Double)
            _HoughMinLine = value
        End Set
    End Property

    Private _HoughGapLines As Double = 20
    Public Property HoughGapLines() As Double
        Get
            Return _HoughGapLines
        End Get
        Set(ByVal value As Double)
            _HoughGapLines = value
        End Set
    End Property


    Private _Cor1 As Global.System.Drawing.Color = Color.Black
    Public Property Cor1() As Global.System.Drawing.Color
        Get
            Return _Cor1
        End Get
        Set(ByVal value As Global.System.Drawing.Color)
            _Cor1 = value
        End Set
    End Property

    Private _Cor2 As Global.System.Drawing.Color = Color.IndianRed
    Public Property Cor2() As Global.System.Drawing.Color
        Get
            Return _Cor2
        End Get
        Set(ByVal value As Global.System.Drawing.Color)
            _Cor2 = value
        End Set
    End Property

    Private _CalibracaoDimX As Double = 10
    Public Property CalibracaoDimX() As Double
        Get
            Return _CalibracaoDimX
        End Get
        Set(ByVal value As Double)
            _CalibracaoDimX = value
        End Set
    End Property

End Class
