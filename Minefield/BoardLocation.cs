namespace Minefield
{
    /// <summary>
    /// Represents a location on the border that can contain a player or a mine
    /// </summary>
    public class BoardLocation
    {
        /// <summary>
        /// Gets or Sets the vertical position (numeric)
        /// </summary>
        public int VerticalPosition { get; set; }

        /// <summary>
        /// Gets or Sets the horizontal position
        /// </summary>
        public char HorizontalPosition { get; set; }

        /// <summary>
        /// Returns a string representing the position
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{HorizontalPosition}:{VerticalPosition}";
        }
    }
}
