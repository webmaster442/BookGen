//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.Configuration
{
    /// <summary>
    /// Template options
    /// </summary>
    public interface IReadOnlyTemplateOptions
    {
        /// <summary>
        /// Ties to get an option from the config
        /// </summary>
        /// <typeparam name="T">Option to cast to type</typeparam>
        /// <param name="setting">Setting name</param>
        /// <param name="value">Casted setting value</param>
        /// <returns>true, if setting found and casted succesfully to target type, otherwise false</returns>
        bool TryGetOption<T>(string setting, out T value) where T : struct;
    }
}
