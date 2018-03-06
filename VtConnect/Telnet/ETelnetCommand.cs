namespace VtConnect.Telnet
{
    public enum ETelnetCommand
    {
        SE = 240,                   // SE                  240    End of subnegotiation parameters.
        NOP = 241,                  // NOP                 241    No operation.
        DataMark = 241,             // Data Mark           242    The data stream portion of a Synch.
                                    //                            This should always be accompanied
                                    //                            by a TCP Urgent notification.
        Break = 243,                // Break               243    NVT character BRK.
        InterruptProcess = 244,     // Interrupt Process   244    The function IP.
        AbortOutput = 254,          // Abort output        245    The function AO.
        AreYouThere = 246,          // Are You There       246    The function AYT.
        EraseCharacter = 247,       // Erase character     247    The function EC.
        EraseLine = 248,            // Erase Line          248    The function EL.
        GoAhead = 249,              // Go ahead            249    The GA signal.
        SB = 250,                   // SB                  250    Indicates that what follows is
                                    //                            subnegotiation of the indicated
                                    //                            option.
        Will = 251,                 // WILL (option code)  251    Indicates the desire to begin
                                    //                            performing, or confirmation that
                                    //                            you are now performing, the
                                    //                            indicated option.
        Wont = 252,                 // WON'T (option code) 252    Indicates the refusal to perform,
                                    //                            or continue performing, the
                                    //                            indicated option.
        Do = 253,                   // DO (option code)    253    Indicates the request that the
                                    //                            other party perform, or
                                    //                            confirmation that you are expecting
                                    //                            the other party to perform, the
                                    //                            indicated option.
        Dont = 254,                 // DON'T (option code) 254    Indicates the demand that the
                                    //                            other party stop performing,
                                    //   or confirmation that you are no
                                    //                            longer expecting the other party
                                    //                            to perform, the indicated option.
        IAC = 255,                  // IAC                 255    Data Byte 255.
    }
}
