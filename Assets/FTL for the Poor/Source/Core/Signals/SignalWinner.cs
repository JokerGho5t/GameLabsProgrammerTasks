namespace Ships.Signals
{
    public readonly struct SignalWinner
    {
        public readonly string WinnerName;

        public SignalWinner(string winnerName)
        {
            WinnerName = winnerName;
        }
    }
}