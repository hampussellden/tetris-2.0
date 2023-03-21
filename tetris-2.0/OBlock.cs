namespace tetris_2._0;
public class OBlock : Block
{
    private readonly Position[][] _tiles =
    {
        new Position[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) }
    };
    public override int Id => 4;
    protected override Position StartOffset => new Position(0, 4);

    protected override Position[][] Tiles => _tiles;

}