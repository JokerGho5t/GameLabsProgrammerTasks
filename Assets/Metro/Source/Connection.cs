namespace Metro
{
    public readonly struct Connection
    {
        public readonly EMetroStation Station;
        public readonly EMetroLine Line;

        public Connection(EMetroStation station, EMetroLine line)
        {
            Station = station;
            Line = line;
        }
    }
}