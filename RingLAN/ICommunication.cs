using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    public delegate void MessageRecievedHandler(object sender, MessageEventArgs args);
    /// <summary>
    /// The interface to all comms methods, whether over RS-232, USB, virtual, et cetera.
    /// </summary>
    public interface ICommunication {
        /// <summary>
        /// Sends a message object
        /// </summary>
        /// <param name="message">The object to send</param>
        void Send(Message message);
        /// <summary>
        /// An event that fires when a new message comes in
        /// </summary>
        event MessageRecievedHandler Recieved; //Fires whenever a message is recieved.
    }
}
