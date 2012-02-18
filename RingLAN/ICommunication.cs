namespace RingLAN {
    public delegate void MessageRecievedHandler(object sender, MessageEventArgs args);

    public delegate void CharacterRecievedHandler(object sender, char character);
    /// <summary>
    /// The interface to all comms methods, whether over RS-232, USB, virtual, et cetera.
    /// </summary>
    public interface ICommunication {
        /// <summary>
        /// Gets or sets the object's parent
        /// </summary>
        Client Parent { get; set; }
        /// <summary>
        /// Sends a message object
        /// </summary>
        /// <param name="message">The object to send</param>
        void Send(Message message);
        /// <summary>
        /// Sends a message object without caring if it's recieved
        /// </summary>
        /// <param name="message">The object to send</param>
        void PassOn(Message message);
        /// <summary>
        /// Performs shutdown.
        /// </summary>
        void Close();
        /// <summary>
        /// An event that fires when a new message comes in
        /// </summary>
        event MessageRecievedHandler Recieved;
        /// <summary>
        /// An Event that fires when a message could not be sent
        /// </summary>
        event MessageRecievedHandler Failed;
        /// <summary>
        /// An event that fires whenever a character is recieved.
        /// </summary>
        event CharacterRecievedHandler CharacterRecieved;
    }
}
