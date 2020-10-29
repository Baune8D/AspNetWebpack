namespace AspNetWebpack.AssetHelpers
{
    /// <summary>
    /// Defines how to load the script.
    /// </summary>
    public enum ScriptLoad
    {
        /// <summary>
        /// The normal way.
        /// </summary>
        Normal,

        /// <summary>
        /// With async on the script tag.
        /// </summary>
        Async,

        /// <summary>
        /// With defer on the script tag.
        /// </summary>
        Defer,

        /// <summary>
        /// With both async and defer on the script tag.
        /// </summary>
        AsyncDefer,
    }
}
