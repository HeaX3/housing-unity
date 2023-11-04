namespace Housing.Extensions
{
    public static class FaceExtensions
    {
        public static Face GetCounterClockwiseFace(this Face face)
        {
            return face switch
            {
                Face.North => Face.West,
                Face.East => Face.North,
                Face.South => Face.East,
                Face.West => Face.South,
                _ => face
            };
        }

        public static Face GetClockwiseFace(this Face face)
        {
            return face switch
            {
                Face.North => Face.East,
                Face.East => Face.South,
                Face.South => Face.West,
                Face.West => Face.North,
                _ => face
            };
        }

        public static Face GetOppositeFace(this Face face)
        {
            return face switch
            {
                Face.Up => Face.Down,
                Face.Down => Face.Up,
                Face.North => Face.South,
                Face.East => Face.West,
                Face.South => Face.North,
                Face.West => Face.East,
                _ => face
            };
        }
    }
}