namespace Core.Elements
{
    #region Enums
    public enum ComputableElementState
    {
        /// <summary>
        /// No state.
        /// </summary>
        Unset = -1,
        /// <summary>
        /// Default state.
        /// </summary>
        Default = 0,
        Computing = 1,
        Computed = 2,
        Failed = 3
    }

    #endregion
}
