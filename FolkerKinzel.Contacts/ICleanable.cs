namespace FolkerKinzel.Contacts
{
    /// <summary>
    /// Interface, das das implementierende Objekt befähigt, sich selbst von Datenmüll zu reinigen.
    /// </summary>
    public interface ICleanable
    {
        /// <summary>
        /// Entfernt Datenmüll aus dem Objekt.
        /// </summary>
        void Clean();

        /// <summary>
        /// True gibt an, dass das Objekt keine verwertbaren Daten enthält.
        /// </summary>
        bool IsEmpty { get; }
    }
}
