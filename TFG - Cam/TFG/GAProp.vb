Public Class GAProp

    Private _NPrateleirasX As Integer = 4
    Public Property NPrateleirasX() As Integer
        Get
            Return _NPrateleirasX
        End Get
        Set(ByVal value As Integer)
            _NPrateleirasX = value
        End Set
    End Property

    Private _NPrateleirasY As Integer = 4
    Public Property NPrateleirasY() As Integer
        Get
            Return _NPrateleirasY
        End Get
        Set(ByVal value As Integer)
            _NPrateleirasY = value
        End Set
    End Property

    Private _NLinhas As Integer = 5
    Public Property NLinhasPrat() As Integer
        Get
            Return _NLinhas
        End Get
        Set(ByVal value As Integer)
            _NLinhas = value
        End Set
    End Property

    Private _NColunas As Integer = 5
    Public Property NColunasPrat() As Integer
        Get
            Return _NColunas
        End Get
        Set(ByVal value As Integer)
            _NColunas = value
        End Set
    End Property

    Private _Geracoes As Integer = 500
    Public Property Geracoes() As Integer
        Get
            Return _Geracoes
        End Get
        Set(ByVal value As Integer)
            _Geracoes = value
        End Set
    End Property

    Private _Populacao As Integer = 50
    Public Property Populacao() As Integer
        Get
            Return _Populacao
        End Get
        Set(ByVal value As Integer)
            _Populacao = value
        End Set
    End Property

    Private _ProMutacao As Double = 10
    Public Property ProMutacao() As Double
        Get
            Return _ProMutacao
        End Get
        Set(ByVal value As Double)
            _ProMutacao = value
        End Set
    End Property

End Class
