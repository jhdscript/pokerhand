Imports HoldemHand

Class MainWindow
    Private _selectedCard As Image

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    End Sub

    Private Sub CardPolo1_Drop(sender As Object, e As DragEventArgs)
        CardPolo1.Source = _selectedCard.Source
    End Sub
    Private Sub CardPolo2_Drop(sender As Object, e As DragEventArgs)
        CardPolo2.Source = _selectedCard.Source
    End Sub
    Private Sub CardFlop1_Drop(sender As Object, e As DragEventArgs)
        CardFlop1.Source = _selectedCard.Source
    End Sub
    Private Sub CardFlop2_Drop(sender As Object, e As DragEventArgs)
        CardFlop2.Source = _selectedCard.Source
    End Sub
    Private Sub CardFlop3_Drop(sender As Object, e As DragEventArgs)
        CardFlop3.Source = _selectedCard.Source
    End Sub
    Private Sub CardFlop4_Drop(sender As Object, e As DragEventArgs)
        CardFlop4.Source = _selectedCard.Source
    End Sub
    Private Sub CardFlop5_Drop(sender As Object, e As DragEventArgs)
        CardFlop5.Source = _selectedCard.Source
    End Sub

    Private Sub Image_PreviewMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Dim img As Image = DirectCast(sender, Image)
        _selectedCard = img
        DragDrop.DoDragDrop(img, img.Source.ToString, DragDropEffects.All)
    End Sub

    Private Sub BtCalc_Click(sender As Object, e As RoutedEventArgs)
        Dim player(8) As Double
        Dim opponent(8) As Double

        Dim c1 As String = GetCardVal(CardPolo1)
        Dim c2 As String = GetCardVal(CardPolo2)
        Dim f1 As String = GetCardVal(CardFlop1)
        Dim f2 As String = GetCardVal(CardFlop2)
        Dim f3 As String = GetCardVal(CardFlop3)
        Dim f4 As String = GetCardVal(CardFlop4)
        Dim f5 As String = GetCardVal(CardFlop5)
        If String.IsNullOrEmpty(c1) Or String.IsNullOrEmpty(c2) Then
            MsgBox("T'as oublié tes cartes polo !")
            Exit Sub
        End If
        If String.IsNullOrEmpty(f1) Or String.IsNullOrEmpty(f2) Or String.IsNullOrEmpty(f3) Then
            MsgBox("Il faut au moins 3 cartes dans le flop polo !")
        End If
        Dim boardCards As List(Of String) = {f1, f2, f3}.ToList
        If String.IsNullOrEmpty(f4) = False Then
            boardCards.Add(f4)
        End If
        If String.IsNullOrEmpty(f5) = False Then
            boardCards.Add(f5)
        End If
        Dim pocket As String = Join({c1, c2}, " ")
        Dim board As String = Join(boardCards.ToArray, " ")

        If Hand.ValidateHand(pocket + " " + board) = False Then
            MsgBox("Y a comme qui dirait un petit problème polo !")
            Exit Sub
        End If
        Dim count As Integer
        Hand.ParseHand(pocket + " " + board, count)

        If count = 0 Or count = 1 Or count = 3 Or count = 4 Or count > 7 Then
            MsgBox("Le nombre de carte est pas bon polo !")
        End If

        Hand.HandPlayerOpponentOdds(pocket, board, player, opponent)
        Dim playerwins As Double = 0.0
        Dim opponentwins As Double = 0.0
        For i As Integer = 0 To 8
            Select Case i
                Case Hand.HandTypes.HighCard
                    TxtPlayerHighCard.Text = FormatPercent(player(i))
                    TxtOpponentHighCard.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.Pair
                    TxtPlayerPair.Text = FormatPercent(player(i))
                    TxtOpponentPair.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.TwoPair
                    TxtPlayerTwoPair.Text = FormatPercent(player(i))
                    TxtOpponentTwoPair.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.Trips
                    TxtPlayer3OfKing.Text = FormatPercent(player(i))
                    TxtOpponent3OfKing.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.Straight
                    TxtPlayerStraight.Text = FormatPercent(player(i))
                    TxtOpponentStraight.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.Flush
                    TxtPlayerFlush.Text = FormatPercent(player(i))
                    TxtOpponentFlush.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.FullHouse
                    TxtPlayerFullHouse.Text = FormatPercent(player(i))
                    TxtOpponentFullHouse.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.FourOfAKind
                    TxtPlayer4OfKing.Text = FormatPercent(player(i))
                    TxtOpponent4OfKing.Text = FormatPercent(opponent(i))
                Case Hand.HandTypes.StraightFlush
                    TxtPlayerStraightFlush.Text = FormatPercent(player(i))
                    TxtOpponentStraightFlush.Text = FormatPercent(opponent(i))
            End Select
            playerwins += player(i) * 100
            opponentwins += opponent(i) * 100
        Next
        TxtPlayerWinSplit.Text = String.Format("{0:##0.0}%", playerwins)
        TxtOpponentWinSplit.Text = String.Format("{0:##0.0}%", opponentwins)

        Dim ppot As Double
        Dim npot As Double
        Hand.HandPotential(Hand.ParseHand(pocket), Hand.ParseHand(board), ppot, npot)
        TxtPotPositive.Text = FormatPercent(ppot)
        TxtPotNegative.Text = FormatPercent(npot)
    End Sub

    Private Function GetCardVal(img As Image) As String
        Dim card As String = Replace(Split(img.Source.ToString, "/").Last.ToUpper, ".PNG", "")
        If card = "BLANK" Then
            Return Nothing
        End If
        Return card
    End Function

    Private Function FormatPercent(v As Double) As String
        If v <> 0.0 Then
            If v * 100 >= 1.0 Then
                Return String.Format("{0:##0.0}%", v * 100.0)
            Else
                Return "<1%"
            End If
        End If
        Return "n/a"
    End Function

    Private Sub BtReset_Click(sender As Object, e As RoutedEventArgs)
        For i As Integer = 0 To 6
            Dim img As Image = Nothing
            Select Case i
                Case 0 : img = CardPolo1
                Case 1 : img = CardPolo2
                Case 2 : img = CardFlop1
                Case 3 : img = CardFlop2
                Case 4 : img = CardFlop3
                Case 5 : img = CardFlop4
                Case 6 : img = CardFlop5
            End Select
            If img IsNot Nothing Then
                img.Source = New BitmapImage(New Uri("Images/blank.png", UriKind.Relative))
            End If
        Next
    End Sub
End Class
