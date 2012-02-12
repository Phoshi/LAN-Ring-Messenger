using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingLAN {
    public class Pending {
        public Message Message;
        public DateTime LastSend;
        public int SendCount;

        public Pending(Message message) {
            Message = message;
            LastSend = DateTime.MinValue;
            SendCount = 0;
        }
    }
}
